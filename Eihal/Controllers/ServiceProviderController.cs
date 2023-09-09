using Eihal.Data;
using Eihal.Data.Entites;
using Eihal.Enums;
using Eihal.Hubs;
using Eihal.Models;
using Eihal.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Eihal.Dto;

namespace Eihal.Controllers
{


    [Route("ServiceProvider")]
    [Authorize(Roles = "ServiceProvider,Beneficiary")]
    public class ServiceProviderController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ServiceProviderController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext dbContext, INotificationService notificationService) : base(dbContext, notificationService)
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


        [Route("Notifications")]
        public IActionResult Notifications()
        {
            var currentUserId = GetUserProfileId();
            var model = _dbContext.Notifications.Where(a => a.AssignedToUserId == currentUserId).OrderByDescending(a => a.CreationDate).ToList();
            return View("Notifications", model);
        }


        [Route("GetUserIncomingRequests")]

        public ActionResult GetUserIncomingRequests(string? name, int? statusId = 0, int? dateId = 0)
        {
            var currentUserId = GetUserProfileId();
            if (string.IsNullOrEmpty(name))
                name = string.Empty;
            var model = _dbContext.ReferralRequests.Include(a => a.Order).Include(a => a.CreatedByUser).Include(a => a.AssignedToUser).Where(a =>
            (a.CreatedByUser.FullName.Contains(name) || name == string.Empty)
            && (statusId == 0 || (int)a.Status == statusId)
            && (dateId == 0 ||
            (dateId == 1 && a.CreationDate.Date == DateTime.Now.Date) ||
            (dateId == 2 && a.CreationDate >= DateTime.Now.AddDays(-7).Date) ||
            (dateId == 3 && a.CreationDate >= DateTime.Now.AddDays(-30).Date)
            )
            && a.AssignedToUserId == currentUserId).OrderByDescending(a => a.CreationDate).ToList();

            return PartialView("_IncomingRequests", model);
        }
        [Route("GetUserOutgoingRequests")]

        public ActionResult GetUserOutgoingRequests(string? name, int? statusId = 0, int? dateId = 0)
        {
            var currentUserId = GetUserProfileId();
            if (string.IsNullOrEmpty(name))
                name = string.Empty;
            var model = _dbContext.ReferralRequests.Include(a => a.Order).Include(a => a.CreatedByUser).Include(a => a.AssignedToUser).Where(a =>
            (a.AssignedToUser.FullName.Contains(name) || name == string.Empty)
            && (statusId == 0 || (int)a.Status == statusId)
            && (dateId == 0 ||
            (dateId == 1 && a.CreationDate.Date == DateTime.Now.Date) ||
            (dateId == 2 && a.CreationDate >= DateTime.Now.AddDays(-7).Date) ||
            (dateId == 3 && a.CreationDate >= DateTime.Now.AddDays(-30).Date)
            )
            && a.CreatedByUserId == currentUserId).OrderByDescending(a => a.CreationDate).ToList();

            return PartialView("_OutgoingRequests", model);
        }

        [Route("ApproveReferal")]

        public ActionResult ApproveReferal(int refId)
        {
            var currentUserId = GetUserProfileId();

            var referralRequest = _dbContext.ReferralRequests.Where(a => a.Id == refId && a.AssignedToUserId == currentUserId && a.Status == ReferralStatusEnum.UnderReview).First();

            referralRequest.Status = ReferralStatusEnum.Approved;
            _dbContext.SaveChanges();
            var referralRequestId = referralRequest.Id;
            string requestNumber = referralRequestId.ToString("#0000");
            PushNewNotification(SharedEnum.NotificationTypeEnum.ApprovedOrder, GetUserProfileId(), referralRequest.CreatedByUserId, requestNumber);


            return Ok();
        }

        [Route("CompleteReferal")]
        public ActionResult CompleteReferal(int refId)
        {
            var currentUserId = GetUserProfileId();

            var referralRequest = _dbContext.ReferralRequests.Where(a => a.Id == refId && a.AssignedToUserId == currentUserId && a.Status == ReferralStatusEnum.Approved).First();

            referralRequest.Status = ReferralStatusEnum.Completed;
            referralRequest.CompletionDate = DateTime.Now;
            _dbContext.SaveChanges();
            //var referralRequestId = referralRequest.Id;
            //string requestNumber = referralRequestId.ToString("#0000");
            //PushNewNotification(SharedEnum.NotificationTypeEnum.ApprovedOrder, GetUserProfileId(), referralRequest.CreatedByUserId, requestNumber);


            return Ok();
        }
        [Route("RejectReferal")]

        public ActionResult RejectReferal(int refId)
        {
            var currentUserId = GetUserProfileId();

            var referralRequest = _dbContext.ReferralRequests.Where(a => a.Id == refId && a.AssignedToUserId == currentUserId && a.Status == ReferralStatusEnum.UnderReview).First();

            referralRequest.Status = ReferralStatusEnum.Rejected;
            _dbContext.SaveChanges();
            var referralRequestId = referralRequest.Id;
            string requestNumber = referralRequestId.ToString("#0000");
            PushNewNotification(SharedEnum.NotificationTypeEnum.RejectOrder, GetUserProfileId(), referralRequest.CreatedByUserId, requestNumber);


            return Ok();
        }



        #endregion


        [Route("AddServices")]
        public IActionResult AddServices()
        {
            //used for filter // view only the priv that's related to the user Practitioner Type

            var currentUserId = GetUserProfileId();
            var userPractitionerTypeId = _dbContext.UserProfiles.Where(a => a.Id == currentUserId).Select(a => a.PractitionerTypeId).FirstOrDefault();

            var clinicalSpecialities =
                           (
                            from c in _dbContext.ClinicalSpecialities
                            where c.IsActive && c.PractitionerTypeId == userPractitionerTypeId
                            select c).ToList();

            return View(clinicalSpecialities);
        }

        [Route("MyServiceCardPartial")]

        public ActionResult MyServiceCardPartial()
        {
            var currentUserId = GetUserProfileId();

            // Assuming you have a list of items to pass to the view
            List<UserServices> model = _dbContext.UserServices.Where(a => a.UserId == currentUserId && a.Status != Enums.ServicesStatusEnum.Deleted).ToList(); // Replace with your logic to fetch the items

            // Render the partial view and return it as HTML content
            //string htmlContent = RenderPartialToString("_CardPartial", model); // Replace with the name of your partial view

            return PartialView("_ServiceCardPartial", model); // Replace with the name of your partial view
        }
        [Route("AllServiceCardPartial")]

        public ActionResult AllServiceCardPartial(string clinicalSpecialitiesIds, string kw)
        {
            var currentUserId = GetUserProfileId();
            var userPractitionerTypeId = _dbContext.UserProfiles.Where(a => a.Id == currentUserId).Select(a => a.PractitionerTypeId).FirstOrDefault();

            var currentUserServices = _dbContext.UserServices.Where(a => a.UserId == currentUserId && a.Status != Enums.ServicesStatusEnum.Deleted).Select(a => a.ServiceId);
            var services = from s in _dbContext.Services
                               //join p in _dbContext.Privillages on s.PrivillageId equals p.Id
                           join c in _dbContext.ClinicalSpecialities on s.ClinicalSpecialityId equals c.Id
                           where s.IsActive && !currentUserServices.Contains(s.Id) && c.PractitionerTypeId == userPractitionerTypeId
                           select s;

            if (clinicalSpecialitiesIds != "All")
            {
                var listOfIDs = clinicalSpecialitiesIds.Split(',').Select(int.Parse).ToList();
                services = services.Where(a => a.ClinicalSpecialityId != null && listOfIDs.Contains(a.ClinicalSpecialityId.Value));
            }
            if (!string.IsNullOrEmpty(kw))
            {
                services = services.Where(a => a.TitleEn.Contains(kw));
            }

            return PartialView("_AllServiceCardPartial", services.Include(a => a.ClinicalSpeciality).ToList()); // Replace with the name of your partial view
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

            PushNewNotification(SharedEnum.NotificationTypeEnum.SendNewService, GetUserProfileId(), _adminUserProfileId, GetShortName());
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
                rejectionReason = userProfile.RejectionReason,
                insuranceAccept = userProfile.InsuranceAccepted,
            };

            return Json(data);
        }


        // If want use this in doctor page please move to other controller with anoynomus and other name
        #region
        [HttpGet]
        [Route("GetUserProfileInfoByUseId")]
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
        #endregion
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


        [Route("GetDistricts")]
        public ActionResult GetDistricts(int cityId)
        {
            var districts = _dbContext.Districts.Where(w => w.CityId == cityId).ToList();
            return Json(districts);
        }

        [HttpGet]
        [Route("SendProfileToReview")]
        public ActionResult SendProfileToReview()
        {
            var userProfile = _dbContext.UserProfiles.Single(w => w.UserId == GetAspNetUserId());
            userProfile.ProfileStatus = ProfileStatus.UnderReview;
            _dbContext.UserProfiles.Update(userProfile);
            _dbContext.SaveChanges();
            PushNewNotification(SharedEnum.NotificationTypeEnum.SendProfileToReview, GetUserProfileId(), _adminUserProfileId, GetShortName());
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
            else
                userProfile.SpecialtiesIds = null;

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



        [HttpPost]
        [Route("UpdateTimeClinicLocation")]
        public IActionResult UpdateTimeClinicLocation(IFormCollection form)
        {
            var userProfileId = GetUserProfileId();
            bool isAddOperation = false;
            var obj = _dbContext.TimeClinicLocations.SingleOrDefault(s => s.UserProfileId == userProfileId);

            if (obj == null)
            {
                obj = new TimeClinicLocation();
                isAddOperation = true;
                obj.UserProfileId = GetUserProfileId();
            }

            if (!string.IsNullOrEmpty(form["ClinicName"]))
                obj.ClinicName = form["ClinicName"];

            // Access form data & fill to user profile object
            if (!string.IsNullOrEmpty(form["CountryId"]))
                obj.CountryId = Convert.ToInt32(form["CountryId"]);

            if (!string.IsNullOrEmpty(form["StateId"]))
                obj.StateId = Convert.ToInt32(form["StateId"]);

            if (!string.IsNullOrEmpty(form["CityId"]))
                obj.CityId = Convert.ToInt32(form["CityId"]);

            if (!string.IsNullOrEmpty(form["DistrictId"]))
                obj.DistrictId = Convert.ToInt32(form["DistrictId"]);

            if (!string.IsNullOrEmpty(form["SundayOpenAt"]) && !string.IsNullOrEmpty(form["SundayClosedAt"]))
            {
                obj.SundayOpenAt = form["SundayOpenAt"];
                obj.SundayClosedAt = form["SundayClosedAt"];
                obj.SundayIsClosed = false;
            }
            else
            {
                obj.SundayIsClosed = form["SundayIsClosed"] == "true" ? true : false;
            }

            if (!string.IsNullOrEmpty(form["MondayOpenAt"]) && !string.IsNullOrEmpty(form["MondayClosedAt"]))
            {
                obj.MondayOpenAt = form["MondayOpenAt"];
                obj.MondayClosedAt = form["MondayClosedAt"];
                obj.MondayIsClosed = false;
            }
            else
            {
                obj.MondayIsClosed = form["MondayIsClosed"] == "true" ? true : false;
            }

            if (!string.IsNullOrEmpty(form["TuesdayOpenAt"]) && !string.IsNullOrEmpty(form["TuesdayClosedAt"]))
            {
                obj.TuesdayOpenAt = form["TuesdayOpenAt"];
                obj.TuesdayClosedAt = form["TuesdayClosedAt"];
                obj.TuesdayIsClosed = false;
            }
            else
            {
                obj.TuesdayIsClosed = form["TuesdayIsClosed"] == "true" ? true : false;
            }

            if (!string.IsNullOrEmpty(form["WednesdayOpenAt"]) && !string.IsNullOrEmpty(form["WednesdayClosedAt"]))
            {
                obj.WednesdayOpenAt = form["WednesdayOpenAt"];
                obj.WednesdayClosedAt = form["WednesdayClosedAt"];
                obj.WednesdayIsClosed = false;
            }
            else
            {
                obj.WednesdayIsClosed = form["WednesdayIsClosed"] == "true" ? true : false;
            }

            if (!string.IsNullOrEmpty(form["ThursdayOpenAt"]) && !string.IsNullOrEmpty(form["ThursdayClosedAt"]))
            {
                obj.ThursdayOpenAt = form["ThursdayOpenAt"];
                obj.ThursdayClosedAt = form["ThursdayClosedAt"];
                obj.ThursdayIsClosed = false;
            }
            else
            {
                obj.ThursdayIsClosed = form["ThursdayIsClosed"] == "true" ? true : false;
            }

            if (!string.IsNullOrEmpty(form["FridayOpenAt"]) && !string.IsNullOrEmpty(form["FridayClosedAt"]))
            {
                obj.FridayOpenAt = form["FridayOpenAt"];
                obj.FridayClosedAt = form["FridayClosedAt"];
                obj.FridayIsClosed = false;
            }
            else
            {
                obj.FridayIsClosed = form["FridayIsClosed"] == "true" ? true : false;
            }

            if (!string.IsNullOrEmpty(form["SaturdayOpenAt"]) && !string.IsNullOrEmpty(form["SaturdayClosedAt"]))
            {
                obj.SaturdayOpenAt = form["SaturdayOpenAt"];
                obj.SaturdayClosedAt = form["SaturdayClosedAt"];
                obj.SaturdayIsClosed = false;
            }
            else
            {
                obj.SaturdayIsClosed = form["SaturdayIsClosed"] == "true" ? true : false;
            }


            if (isAddOperation)
                _dbContext.TimeClinicLocations.Add(obj);
            else
                _dbContext.TimeClinicLocations.Update(obj);
            _dbContext.SaveChanges();
            return Ok("Updated successfully.");
        }

        [HttpGet]
        [Route("GetTimeClinicLocation")]
        public ActionResult GetTimeClinicLocation()
        {
            var userProfileId = GetUserProfileId();
            var timeClinicLocation = _dbContext.TimeClinicLocations.
                                                             Include(i => i.State)
                                                            .Include(i => i.Country)
                                                            .Include(i => i.City)
                                                            .Include(i => i.Districts)
                                                            .FirstOrDefault(w => w.UserProfileId == userProfileId);
            if (timeClinicLocation != null)
            {
                // Your logic to retrieve the necessary data
                var data = new
                {
                    timeClinicLocation.ClinicName,
                    timeClinicLocation.SundayOpenAt,
                    timeClinicLocation.SundayClosedAt,
                    timeClinicLocation.SundayIsClosed,
                    timeClinicLocation.MondayOpenAt,
                    timeClinicLocation.MondayClosedAt,
                    timeClinicLocation.MondayIsClosed,
                    timeClinicLocation.TuesdayOpenAt,
                    timeClinicLocation.TuesdayClosedAt,
                    timeClinicLocation.TuesdayIsClosed,
                    timeClinicLocation.WednesdayOpenAt,
                    timeClinicLocation.WednesdayClosedAt,
                    timeClinicLocation.WednesdayIsClosed,
                    timeClinicLocation.ThursdayOpenAt,
                    timeClinicLocation.ThursdayClosedAt,
                    timeClinicLocation.ThursdayIsClosed,
                    timeClinicLocation.FridayOpenAt,
                    timeClinicLocation.FridayClosedAt,
                    timeClinicLocation.FridayIsClosed,
                    timeClinicLocation.SaturdayOpenAt,
                    timeClinicLocation.SaturdayClosedAt,
                    timeClinicLocation.SaturdayIsClosed,
                    timeClinicLocation.CountryId,
                    timeClinicLocation.StateId,
                    timeClinicLocation.CityId,
                    timeClinicLocation.DistrictId,
                    CountryName = timeClinicLocation?.Country?.TitleEn,
                    StateName = timeClinicLocation?.State?.TitleEn,
                    CityName = timeClinicLocation?.City?.TitleEn,
                    DistrictName = timeClinicLocation?.Districts?.TitleEn
                };
                return Json(data);
            }
            return Json("");
        }


        [HttpGet]
        [Route("AddInsuranceCompany")]
        public ActionResult AddInsuranceCompany(string companyId)
        {
            int id = Convert.ToInt32(companyId);

            var userProfileId = GetUserProfileId();

            var userCompany = new UserCompany();
            userCompany.UserProfileId = userProfileId;
            userCompany.InsuranceCompanyId = id;

            var uc = _dbContext.UserCompanies
                                     .FirstOrDefault(w => w.UserProfileId == userProfileId && w.InsuranceCompanyId == id);

            var user = _dbContext.UserProfiles.Single(s => s.Id == userProfileId);
            user.InsuranceAccepted = true;
            _dbContext.UserProfiles.Update(user);

            if (uc == null)
            {
                _dbContext.UserCompanies.Add(userCompany);
                _dbContext.SaveChanges();
                return Json("Added successfully.");
            }
            else
                return Json("Already Exists.");
        }


        [HttpGet]
        [Route("GetUserCompanies")]
        public ActionResult GetUserCompanies()
        {
            var userProfileId = GetUserProfileId();
            var result = _dbContext.UserCompanies
                                   .Include(i => i.Company)
                                   .Where(w => w.UserProfileId == userProfileId).Select(s => s.Company).ToList();
            return Json(result);
        }


        [HttpPost]
        [Route("DeleteInsuranceCompany")]
        public ActionResult DeleteInsuranceCompany(int companyId)
        {
            var userProfileId = GetUserProfileId();
            var obj = _dbContext.UserCompanies.Single(w => w.InsuranceCompanyId == companyId && w.UserProfileId == userProfileId);
            _dbContext.UserCompanies.Remove(obj);
            _dbContext.SaveChanges();

            var isAnyCompanyAdded = _dbContext.UserCompanies.Any(w => w.UserProfileId == userProfileId);
            var user = _dbContext.UserProfiles.Single(s => s.Id == userProfileId);
            if (isAnyCompanyAdded)
            {
                user.InsuranceAccepted = true;
            }
            else
                user.InsuranceAccepted = false;

            _dbContext.UserProfiles.Update(user);
            _dbContext.SaveChanges();
            return Json("Deleted Successfully");
        }



        [Route("GetAvailablePrivileges")]

        public ActionResult GetAvailablePrivileges()
        {
            // Assuming you have a list of items to pass to the view
            var model = _dbContext.Services
            .Join(_dbContext.UserServices,
                s => s.Id,
                us => us.ServiceId,
                (s, us) => new SupportServiceModal
                { ServiceId = s.Id, TitleEn = s.TitleEn, TitleAr = s.TitleAr, Status = us.Status })
                .Where(us => us.Status == ServicesStatusEnum.Approved
                ) // Include columns you want from both tables
            .Distinct()
            .ToList();


            // Render the partial view and return it as HTML content
            //string htmlContent = RenderPartialToString("_CardPartial", model); // Replace with the name of your partial view

            return PartialView("PrivilegesOrder", model); // Replace with the name of your partial view
        }


        [Route("GetAvailableDoctors")]
        public ActionResult GetAvailableDoctors(string serviceIds, string name, int cityId, int disctrictId)
        {

            var servicesIds = serviceIds?.Split(',')?.Select(Int32.Parse)?.ToList();
            var servicesIdLength = servicesIds.Count();

            var ids = _dbContext.UserServices
                                    .Where(a => servicesIds.Contains(a.ServiceId))
                                    .GroupBy(us => us.UserId)
                                    .Where(g => g.Select(us => us.ServiceId).Distinct().Count() == servicesIdLength)
                                    .Select(g => g.Key)
                                    .ToList();

            name = string.IsNullOrEmpty(name) ? string.Empty : name;

            var doctors = _dbContext.UserProfiles
                                    .Include(a => a.TimeClinicLocation)
                                    .ThenInclude(a => a.City)
                                    .Include(a => a.InsuranceCompanies)
                                    .Include(x => x.Certifications)
                                    .ThenInclude(a => a.Degree)
                                    .Include(a => a.PractitionerType)
                                    .Include(x => x.City)
                                    .Where(a => a.ProfileStatus == ProfileStatus.Active && a.AccountTypeId == 1
                                         && (name == String.Empty || a.FullName.Contains(name))
                                         && (ids.Contains(a.Id))
                                         && (cityId == 0 || a.TimeClinicLocation.CityId == cityId)
                                         && (disctrictId == 0 || a.TimeClinicLocation.DistrictId == disctrictId)).ToList();



            return PartialView("AvailableDoctorsList", doctors); // Replace with the name of your partial view

        }

        [HttpPost]
        [Route("SendOrderRequest")]
        public IActionResult SendOrderRequest([FromBody] OrderDetailsModal orderDetailsModal)
        {
            if (orderDetailsModal == null)
            {
                return BadRequest("OrderDetail data is null.");
            }

            var servicesIds = orderDetailsModal.SelectedServicesIds?.Split(',')?.Select(Int32.Parse)?.ToList();
            var services = _dbContext.Services.Where(a => servicesIds.Contains(a.Id)).ToList();

            var orderDetail = new OrderDetail()
            {
                PatientName = orderDetailsModal.FullName,
                PhoneNumber = orderDetailsModal.Phone,
                Email = string.IsNullOrEmpty(orderDetailsModal.Email) ? "" : orderDetailsModal.Email,
                Age = orderDetailsModal.Age != null ? (Age)Convert.ToInt32(orderDetailsModal.Age) : Age.Undefined,
                ChronicDisease = string.IsNullOrEmpty(orderDetailsModal.ChronicDisease) ? "" : orderDetailsModal.ChronicDisease,
                Services = services,
                DoctorId = Convert.ToInt32(orderDetailsModal.DoctorId),
                CountryId = string.IsNullOrEmpty(orderDetailsModal.Country) ? null : Convert.ToInt32(orderDetailsModal.Country),
                CityId = string.IsNullOrEmpty(orderDetailsModal.City) ? null : Convert.ToInt32(orderDetailsModal.City),
                StateId = string.IsNullOrEmpty(orderDetailsModal.State) ? null : Convert.ToInt32(orderDetailsModal.State)
            };

            _dbContext.OrderDetails.Add(orderDetail);
            var orderId = _dbContext.SaveChanges();


            var vatValue = _dbContext.GeneralSettings.Single().VatValue;
            var createdByUserId = GetUserProfileId();

            var referralRequest = new ReferralRequest()
            {
                Status = ReferralStatusEnum.UnderReview,
                CreationDate = DateTime.Now,
                CreatedByUserId = createdByUserId,
                AssignedToUserId = Convert.ToInt32(orderDetailsModal.DoctorId),
                OrderId = orderDetail.Id
            };

            _dbContext.ReferralRequests.Add(referralRequest);
            _dbContext.SaveChanges();


            var orderServiceDetails = new List<OrderServiceDetail>();
            foreach (var serviceId in servicesIds)
            {
                var userServiceDetails = _dbContext.UserServices
                                                   .Include(i => i.Service)
                                                   .Where(w => w.UserId == orderDetail.DoctorId)
                                                   .Single();

                var price = userServiceDetails.Price;
                var fee = userServiceDetails.Fee;

                var obj = new OrderServiceDetail()
                {
                    OrderDetailId = orderDetail.Id,
                    Price = price,
                    Fee = fee,
                    ServiceId = serviceId,
                    TitleAr = userServiceDetails.TitleAr,
                    TitleEn = userServiceDetails.TitleEn,
                };
                orderServiceDetails.Add(obj);
            }
            _dbContext.OrderServiceDetails.AddRange(orderServiceDetails);
            _dbContext.SaveChanges();

            var referralRequestId = referralRequest.Id;
            string requestNumber = referralRequestId.ToString("#0000");

            PushNewNotification(SharedEnum.NotificationTypeEnum.NewOrder, GetUserProfileId(), Convert.ToInt32(orderDetailsModal.DoctorId), requestNumber);


            // Return a response indicating success

            return Ok("Order Send successfully.");
        }


        [HttpGet]
        [Route("ExportReport")]
        [AllowAnonymous]
        public ActionResult ExportReport(int referralRequestId)
        {
            PdfReportGenerator reportGenerator = new PdfReportGenerator();
            // Get Logo Image
            string imagePath = GetFileFullPath(_webHostEnvironment, "images", "logo.png");

            // Set Title
            string reportName = string.Format("Invoice" + DateTime.Now.ToString("yyyyMMdd") + "-" + ".pdf");

            // Get Referral Requests 
            var referralReq = _dbContext.ReferralRequests.Include(i => i.Order)
                                                 .Include(i => i.AssignedToUser)
                                                 .Include(i => i.CreatedByUser)
                                                 .Single(w => w.Id == referralRequestId);

            // Get Clinic Name
            var clinicName = _dbContext.TimeClinicLocations
                                                .Where(w => w.UserProfileId == referralReq.AssignedToUserId)
                                                .Select(s => s.ClinicName)
                                                .FirstOrDefault();


            // Get Order Details
            var orderDetails = _dbContext.OrderDetails
                                         .Include(i => i.Services)
                                         .Where(w => w.Id == referralReq.OrderId)
                                         .ToList();




            var reportDto = new ReportDto();

            reportDto.MasterDetails = new MasterDetailsDto()
            {
                OrderDate = referralReq.CompletionDate.Value.ToString("MM/dd/yyyy"),
                DoctorName = referralReq.AssignedToUser.FullName,
                ClinicName = clinicName ?? "Not Defined Yet",
                InvoiceNumber = referralReq.Id.ToString("#0000"),
                PatientName = referralReq.Order.PatientName,
                RequestFrom = referralReq.CreatedByUser.FullName
            };

            var vatValue = Convert.ToDouble(_dbContext.GeneralSettings.Single().VatValue);
            List<DataTableDto> dataSource = new List<DataTableDto>();
            foreach (var order in orderDetails)
            {
                int totalQty = 0;
                double totalUnitPrice = 0;
                double totalPrice = 0;
                double totalVatPercentage = 0;
                double totalVatValue = 0;
                double totalNetWithVat = 0;
                foreach (var svc in order.Services)
                {
                    var dataTableDto = new DataTableDto();
                    dataTableDto.ServiceCode = svc.Id.ToString();
                    dataTableDto.ServiceDesc = svc.TitleAr;
                    dataTableDto.Qty = 1;

                    // Total Qty
                    totalQty = totalQty + dataTableDto.Qty;

                    dataTableDto.UnitPrice = _dbContext.UserServices.Single(w => w.ServiceId == svc.Id && w.UserId == referralReq.AssignedToUserId).Price;
                    // Total totalPrice
                    totalUnitPrice = totalUnitPrice + dataTableDto.UnitPrice;

                    dataTableDto.Total = dataTableDto.UnitPrice * dataTableDto.Qty;
                    // Total totalVatPercentage
                    totalPrice = totalPrice + dataTableDto.Total;

                    dataTableDto.VatPercentage = vatValue;
                    // Total totalVatPercentage
                    totalVatPercentage = totalVatPercentage + dataTableDto.VatPercentage;

                    dataTableDto.VatValue = (dataTableDto.Total * vatValue) / 100;

                    // Total totalVatPercentage
                    totalVatValue = totalVatValue + dataTableDto.VatValue;

                    dataTableDto.NetWithVat = dataTableDto.Total - dataTableDto.VatValue;

                    // Total totalNetWithVat
                    totalNetWithVat = totalNetWithVat + dataTableDto.NetWithVat;

                    dataSource.Add(dataTableDto);
                }
                var total = new DataTableDto();
                // For Total Rows
                total.ServiceCode = "#";
                total.ServiceDesc = "Total Amount";
                total.Qty = totalQty;
                total.UnitPrice = totalUnitPrice;
                total.Total = totalPrice;
                total.VatValue = totalVatValue;
                total.VatPercentage = totalVatPercentage;
                total.NetWithVat = totalNetWithVat;
                dataSource.Add(total);

            }

            reportDto.DataTable = dataSource;
            MemoryStream reportFile = reportGenerator.GenerateReport(imagePath, reportDto);

            // Return File
            return File(reportFile, "application/pdf", reportName);
        }
    }
}

public class Customer
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string ZipCode { get; set; }
}
