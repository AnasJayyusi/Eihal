using Microsoft.AspNetCore.Mvc;
using Eihal.Data;
using Eihal.Data.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Eihal.Controllers
{
    [Route("Beneficiary")]
    [Authorize(Roles = "Beneficiary")]
    public class BeneficiaryController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BeneficiaryController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext dbContext) : base(dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("Profile")]
        public IActionResult Profile()
        {
            return View();
        }
        [HttpGet]
        [Route("GetUserImage")]
        public ActionResult GetUserImage()
        {
            var userId = GetUserProfileId();
            var userProfilePath = _dbContext.UserProfiles.Where(w => w.Id == userId).Select(a => a.ProfilePicturePath).FirstOrDefault();
            if (userProfilePath == null)
            {
                return Ok("/users/images/Default-User-Profile.jpg");
            }
            return Ok(userProfilePath);
        }
        public IActionResult Referrals()
        {
            return View();
        }

        [Route("AddServices")]
        public IActionResult AddServices()
        {
            var currentUserId = GetUserProfileId();
            var currentUserServices = _dbContext.UserServices.Where(a => a.Status != Enums.ServicesStatusEnum.Deleted).Select(a => a.ServiceId);
            var services = _dbContext.Services.Where(a => a.IsActive && !currentUserServices.Contains(a.Id)).ToList();
            //var services = _dbContext.Services.Where(a => a.IsActive).ToList();
            return View(services);
        }

        [Route("DeleteUserService")]
        public ActionResult DeleteUserService(int Id)
        {
            // Assuming you have a list of items to pass to the view
            var currentUserId = GetUserProfileId();
            var userService = _dbContext.UserServices.Where(a => a.UserId == currentUserId && a.Id == Id).FirstOrDefault();
            if (userService != null)
            {
                _dbContext.UserServices.Remove(userService);
                _dbContext.SaveChanges();
            }
            // Render the partial view and return it as HTML content
            //string htmlContent = RenderPartialToString("_CardPartial", model); // Replace with the name of your partial view

            return Ok(); // Replace with the name of your partial view
        }

        [Route("AddUserService")]
        [HttpPost]
        public IActionResult AddUserService(int serviceId, double price)
        {
            var service = _dbContext.Services.Where(a => a.Id == serviceId && a.IsActive).FirstOrDefault();
            if (service == null)
                throw new Exception("service not available");

            UserServices userServices = new UserServices();
            userServices.UserId = GetUserProfileId();
            userServices.TitleEn = service.TitleEn;
            userServices.TitleAr = service.TitleAr;
            userServices.Status = Enums.ServicesStatusEnum.Pending;
            userServices.ServiceId = service.Id;
            userServices.Price = price;
            userServices.Fee = service.Fee;
            userServices.CreatedOn = DateTime.Now;
            _dbContext.UserServices.Add(userServices);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("GetUserProfileInfo")]
        public ActionResult GetUserProfileInfo()
        {

            var userProfile = _dbContext.UserProfiles.FirstOrDefault(w => w.UserId == GetAspNetUserId());
            var specialityNames = "";
            // Your logic to retrieve the necessary data
            if (userProfile.SpecialtiesIds != null)
            {
                List<int> specialityIds = userProfile.SpecialtiesIds.Split(',').Select(int.Parse).ToList();

                // This Code Temp We Should Depnd On Select2 
                if (specialityIds != null)
                {
                    var specialities = _dbContext.Specialties
                                   .Where(t => specialityIds.Contains(t.Id)).Select(a => new
                                   {
                                       a.TitleEn
                                   }).ToList();
                    if (specialities.Any())
                        specialityNames = String.Join(",", specialities.Select(a => a.TitleEn));
                }
            }

            var data = new
            {
                fullname = userProfile?.FullName,
                numberPatients = userProfile.NumOfPatients ?? 0,
                review = userProfile.Reviews ?? 0,
                insurance = userProfile.InsuranceAccepted ?? false,
                bio = userProfile.Bio ?? "No Bio yet.",
                speciality = specialityNames ?? "No Speciality added yet.",
                profilePicturePath = userProfile.ProfilePicturePath,
                profileStatus = userProfile.ProfileStatus.ToString(),
                rejectionReason = userProfile.RejectionReason
            };

            return Json(data);
        }

        [HttpGet]
        [Route("GetFullUserProfileInfo")]
        public ActionResult GetFullUserProfileInfo()
        {
            _loggedAspNetUserId = GetAspNetUserId();
            var userProfile = _dbContext.UserProfiles.Include(i => i.State).Include(i => i.City).FirstOrDefault(w => w.UserId == GetAspNetUserId());

            // Your logic to retrieve the necessary data
            var data = new
            {
                FullName = userProfile?.FullName,
                Bio = userProfile?.Bio,
                AccountTypeId = userProfile.AccountTypeId,
                PractitionerTypeId = userProfile.PractitionerTypeId,
                PhoneNumber = userProfile.PhoneNumber,
                Email = userProfile.Email,
                ProfessionalRankId = userProfile.ProfessionalRankId,
                CountryId = userProfile.CountryId,
                StateId = userProfile.StateId,
                StateName = userProfile?.State?.TitleEn,
                CityId = userProfile.CityId,
                CityName = userProfile?.City?.TitleEn,
                SpecialtiesTitlesEn = userProfile.SpecialtiesTitlesEn,
                ProfilePicturePath = userProfile.ProfilePicturePath,
                speciliaties = userProfile.SpecialtiesIds

            };

            return Json(data);
        }

        [HttpGet]
        [Route("GetUserProfessionalRanksDDL")]
        public ActionResult GetUserProfessionalRanksDDL()
        {
            _loggedAspNetUserId = GetAspNetUserId();
            var practitionerTypeId = _dbContext.UserProfiles
                                                .Where(u => u.UserId == GetAspNetUserId())
                                                .Select(u => u.PractitionerTypeId);

            var professionalRanks = _dbContext.ProfessionalRanks
                                               .Where(w => w.IsActive && practitionerTypeId.Contains(w.PractitionerTypeId))
                                               .Select(s => new { Id = s.Id, TitleEn = s.TitleEn, TitleAr = s.TitleAr })
                                               .ToList();
            return Json(professionalRanks);
        }

        [HttpGet]
        [Route("GetUserSpecialtiesDDL")]
        public ActionResult GetUserSpecialtiesDDL()
        {
            _loggedAspNetUserId = GetAspNetUserId();
            var practitionerTypeId = _dbContext.UserProfiles
                                                .Where(u => u.UserId == GetAspNetUserId())
                                                .Select(u => u.PractitionerTypeId);

            var Specialties = _dbContext.Specialties
                                               .Where(w => w.IsActive && practitionerTypeId.Contains(w.PractitionerTypeId))
                                               .Select(s => new { id = s.Id, TitleEn = s.TitleEn, TitleAr = s.TitleAr })
                                               .ToList();
            return Json(Specialties);
        }

        [HttpGet]
        [Route("GetCountriesDDL")]
        public ActionResult GetCountriesDDL()
        {
            var countries = _dbContext.Countries.ToList();
            return Json(countries);
        }

        [HttpGet]
        [Route("GetStatesDDL")]
        public ActionResult GetStatesDDL(int countryId)
        {
            var countries = _dbContext.States.Where(w => w.CountryId == countryId).ToList();
            return Json(countries);
        }

        [Route("GetCitiesDDL")]
        public ActionResult GetCitiesDDL(int stateId)
        {
            var cities = _dbContext.Cities.Where(w => w.StateId == stateId).ToList();
            return Json(cities);
        }

        [HttpGet]
        [Route("SendProfileToReview")]
        public ActionResult SendProfileToReview()
        {
            var userProfile = _dbContext.UserProfiles.Single(w => w.UserId == GetAspNetUserId());
            userProfile.ProfileStatus = ProfileStatus.UnderReview;
            _dbContext.UserProfiles.Update(userProfile);
            _dbContext.SaveChanges();
            return Json(ProfileStatus.UnderReview.ToString());
        }

        //[HttpPost]
        //public JsonResult Index(string kw)
        //{
        //    if (string.IsNullOrEmpty(kw))
        //    {
        //        kw = "";
        //    }
        //    var compaines = new List<InsuranceCompany>()
        //    {

        //        new InsuranceCompany {Id=1,NameEn="InsuranceCompany1",NameAr="شركة التامين الاولى",Base64Image="data:image/png;base64,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" },
        //        new InsuranceCompany {Id=2,NameEn="InsuranceCompany2",NameAr="شركة التامين الثاني" },
        //};
        //    var Name = (from c in compaines
        //                where c.NameAr.Contains(kw) || c.NameEn.Contains(kw)
        //                select new { c.Id, c.NameAr, c.NameEn, c.Base64Image });
        //    return Json(Name);
        //}

        [HttpPost]
        [Route("UpdateUserProfile")]
        public IActionResult UpdateUserProfile(IFormCollection form)
        {
            _loggedAspNetUserId = GetAspNetUserId();
            var userProfile = _dbContext.UserProfiles.Single(w => w.UserId == GetAspNetUserId());


            // Handle image file
            var imageFile = form.Files["image"];



            // Access form data & fill to user profile object
            userProfile.FullName = form["FullName"];
            userProfile.Bio = form["Bio"];

            if (!string.IsNullOrEmpty(form["ProfessionalRankId"]))
                userProfile.ProfessionalRankId = Convert.ToInt32(form["ProfessionalRankId"]);

            if (!string.IsNullOrEmpty(form["CountryId"]))
                userProfile.CountryId = Convert.ToInt32(form["CountryId"]);

            if (!string.IsNullOrEmpty(form["StateId"]))
                userProfile.StateId = Convert.ToInt32(form["StateId"]);

            if (!string.IsNullOrEmpty(form["CityId"]))
                userProfile.CityId = Convert.ToInt32(form["CityId"]);

            if (!string.IsNullOrEmpty(form["SpecialtiesIds"]))
                userProfile.SpecialtiesIds = form["SpecialtiesIds"];

            if (imageFile != null)
                userProfile.ProfilePicturePath = StoreImageFilePathInDatabase(imageFile);

            _dbContext.UserProfiles.Update(userProfile);
            _dbContext.SaveChanges();
            return Ok("Updated successfully.");
        }

        #region Certifications
        [HttpGet]
        [Route("GetCertifications")]
        public ActionResult GetCertifications()
        {
            var userId = GetUserProfileId();
            var attachments = _dbContext.Certifications.Include(i => i.Degree).Where(w => w.UserProfileId == userId).ToList();
            return Json(attachments);
        }
        private string StoreImageFilePathInDatabase(IFormFile profileImage)
        {
            // File extension
            string extension = Path.GetExtension(profileImage.FileName);
            // Get the root path of the web application
            string webRootPath = _webHostEnvironment.WebRootPath;

            // Define the relative path to the image directory within the wwwroot folder
            string imagePath = Path.Combine("users", "images");

            // Create the full path to save the image
            string uploadPath = Path.Combine(webRootPath, imagePath);

            // Create the directory if it doesn't exist
            Directory.CreateDirectory(uploadPath);

            // Generate a unique filename for the image (you can use your own logic here)
            string uniqueFileName = GetAspNetUserId();

            // Combine the upload path with the unique filename
            string filePath = Path.Combine(uploadPath, uniqueFileName) + extension;

            // Save the image file
            using (var imageStream = new FileStream(filePath, FileMode.Create))
            {
                profileImage.CopyTo(imageStream);
            }

            // Path To Save In Database
            return $"/users/images/{uniqueFileName}{extension}";
        }

        [HttpGet]
        [Route("GetDegrees")]
        public ActionResult GetDegrees()
        {
            var degrees = _dbContext.Degrees.Where(w => w.IsActive).ToList();
            return Json(degrees);
        }
        #endregion

    }
}
