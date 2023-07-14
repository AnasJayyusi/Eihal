using Eihal.Data;
using Eihal.Data.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Eihal.Controllers
{

    [Route("ServiceProvider")]
    [Authorize(Roles = "ServiceProvider")]
    public class ServiceProviderController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ServiceProviderController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext dbContext) : base(dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("Profile")]
        public IActionResult Profile()
        {
            SetImagePathInCookies();
            return View();
        }
        #region Referrals
        [Route("Referrals")]
        public IActionResult Referrals()
        {
            return View();
        }


        [Route("GetUserIncomingRequests")]

        public ActionResult GetUserIncomingRequests(string? name,int? statusId=0,int? dateId=0)
        {
            var currentUserId = GetUserProfileId();
            if (string.IsNullOrEmpty(name))
                name = string.Empty;
            var model = _dbContext.ReferralRequests.Include(a=>a.Service).Include(a=>a.CreatedByUser).Include(a=>a.AssignedToUser).Where(a =>
            (a.CreatedByUser.FullName.Contains(name) ||name==string.Empty) 
            &&(statusId ==0 || (int)a.Status==statusId)
            &&(dateId==0 ||
            (dateId == 1 && a.CreationDate.Date == DateTime.Now.Date) ||
            (dateId == 2 && a.CreationDate >= DateTime.Now.AddDays(-7).Date  )||
            (dateId == 3 && a.CreationDate >= DateTime.Now.AddDays(-30).Date  )
            )
            && a.AssignedToUserId == currentUserId).ToList(); 

            return PartialView("_IncomingRequests", model);
        }   
        [Route("GetUserOutgoingRequests")]

        public ActionResult GetUserOutgoingRequests(string? name,int? statusId=0,int? dateId=0)
        {
            var currentUserId = GetUserProfileId();
            if (string.IsNullOrEmpty(name))
                name = string.Empty;
            var model = _dbContext.ReferralRequests.Include(a=>a.Service).Include(a=>a.CreatedByUser).Include(a=>a.AssignedToUser).Where(a =>
            (a.AssignedToUser.FullName.Contains(name) ||name==string.Empty) 
            &&(statusId ==0 || (int)a.Status==statusId)
            &&(dateId==0 ||
            (dateId == 1 && a.CreationDate.Date == DateTime.Now.Date) ||
            (dateId == 2 && a.CreationDate >= DateTime.Now.AddDays(-7).Date  )||
            (dateId == 3 && a.CreationDate >= DateTime.Now.AddDays(-30).Date  )
            )
            && a.CreatedByUserId == currentUserId).ToList(); 

            return PartialView("_OutgoingRequests", model);
        }

        #endregion


        [Route("AddServices")]
        public IActionResult AddServices()
        {
            var currentUserId = GetUserProfileId();
            var currentUserServices = _dbContext.UserServices.Where(a => a.Status != Enums.ServicesStatusEnum.Deleted).Select(a => a.ServiceId);
            var services = _dbContext.Services.Where(a => a.IsActive && !currentUserServices.Contains(a.Id)).ToList();
            //var services = _dbContext.Services.Where(a => a.IsActive).ToList();
            return View(services);
        }
       
        [Route("MyServiceCardPartial")]

        public ActionResult MyServiceCardPartial()
        {
            // Assuming you have a list of items to pass to the view
            List<UserServices> model = _dbContext.UserServices.Where(a => a.Status != Enums.ServicesStatusEnum.Deleted).ToList(); // Replace with your logic to fetch the items

            // Render the partial view and return it as HTML content
            //string htmlContent = RenderPartialToString("_CardPartial", model); // Replace with the name of your partial view

            return PartialView("_ServiceCardPartial", model); // Replace with the name of your partial view
        }
        [Route("AllServiceCardPartial")]

        public ActionResult AllServiceCardPartial()
        {
            // Assuming you have a list of items to pass to the view
            var currentUserId = GetUserProfileId();
            var currentUserServices = _dbContext.UserServices.Where(a => a.Status != Enums.ServicesStatusEnum.Deleted).Select(a => a.ServiceId);
            var services = _dbContext.Services.Where(a => a.IsActive && !currentUserServices.Contains(a.Id)).ToList();
            // Render the partial view and return it as HTML content
            //string htmlContent = RenderPartialToString("_CardPartial", model); // Replace with the name of your partial view

            return PartialView("_AllServiceCardPartial", services); // Replace with the name of your partial view
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
        [Route("GetUserProfileInfo")]
        public ActionResult GetUserProfileInfoByUseId(string userId)
        {

            var userProfile = _dbContext.UserProfiles.FirstOrDefault(w => w.UserId == userId);
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
        [Route("GetSelectedSpecialties")]
        public ActionResult GetSpecialtiesSelectedOptions()
        {
            _loggedAspNetUserId = GetAspNetUserId();

            var selectedOptions = _dbContext.UserProfiles.Where(w => w.UserId == _loggedAspNetUserId)
                                        .Select(s => new { id = s.Id, text = s.SpecialtiesTitlesAr, textAr = s.SpecialtiesTitlesEn });

            return Json(selectedOptions);
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

            if (!string.IsNullOrEmpty(userProfile.SpecialtiesIds))
            {
                var specialtiesIds = userProfile.SpecialtiesIds.Split(',')
                                   .Select(int.Parse)
                                   .ToList();
                userProfile.SpecialtiesTitlesAr = string.Join(",", _dbContext.Specialties.Where(w => specialtiesIds.Contains(w.Id)).Select(x => x.TitleAr));
                userProfile.SpecialtiesTitlesEn = string.Join(",", _dbContext.Specialties.Where(w => specialtiesIds.Contains(w.Id)).Select(x => x.TitleEn));
            }




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

