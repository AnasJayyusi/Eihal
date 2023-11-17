using Eihal.Data;
using Eihal.Data.Entites;
using Eihal.Hubs;
using Eihal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;
using static Eihal.Areas.Identity.Pages.Account.LoginModel;

namespace Eihal.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext dbContext, ILogger<HomeController> logger, INotificationService notificationService) : base(dbContext, notificationService)
        {
            _logger = logger;
        }

        #region Handling Routing 
        public IActionResult Index()
        {
            if (User.IsInRole("Administrator"))
                return RedirectToAction("Dashboard", "Admin");
            return View();
        }
        public IActionResult MyProfile()
        {
            if (User.IsInRole(UserRolesEnum.Beneficiary.ToString()) || User.IsInRole(UserRolesEnum.ServiceProvider.ToString()))
            {
                return RedirectToAction("Profile", "ServiceProvider");
            }
            if (User.IsInRole(UserRolesEnum.Administrator.ToString()))
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            else return View();
        }
        #endregion

        #region About Us
        public IActionResult About()
        {
            return View();
        }
        #endregion

        #region Contact Us
        [HttpPost]
        public ActionResult AddNewFeedback(string name, string contact, string email, string message)
        {
            Feedback feedback = new Feedback();
            feedback.Name = name;
            feedback.Phone = contact;
            feedback.Email = email;
            feedback.Message = message;
            _dbContext.Feedbacks.Add(feedback);
            _dbContext.SaveChanges();
            return Ok("Added Successfully !!");
        }
        public IActionResult Contact()
        {
            return View();
        }
        #endregion

        #region Doctors 
        public IActionResult Doctors(string name, int serviceId, int cityId, int disctrictId)
        {
            List<int> ids = new List<int>();
            if (serviceId > 0)
            {
                ids = _dbContext.UserServices.Where(a => a.ServiceId == serviceId).Select(a => a.UserId).ToList();

            }
            name = string.IsNullOrEmpty(name) ? string.Empty : name;
            var doctors = _dbContext.UserProfiles.Include(a => a.TimeClinicLocation)
                                                 .ThenInclude(a => a.City)
                                                 .Include(a => a.InsuranceCompanies)
                                                 .Include(x => x.Certifications)
                                                 .ThenInclude(a => a.Degree)
                                                 .Include(a => a.PractitionerType)
                                                 .Include(x => x.City).Where(a => a.ProfileStatus == ProfileStatus.Active && a.AccountTypeId == 1
                                                  && (name == String.Empty || a.FullName.Contains(name))
                                                  && (serviceId == 0 || ids.Contains(a.Id))
                                                  && (cityId == 0 || a.TimeClinicLocation.CityId == cityId)
                                                  && (disctrictId == 0 || a.TimeClinicLocation.DistrictId == disctrictId)).ToList();
            return View(doctors);
        }
        public ActionResult FillDoctorsList(string name, int serviceId, int cityId, int disctrictId, int sortBy, int insuranceType)
        {
            List<int> ids = new List<int>();
            if (serviceId > 0)
            {
                ids = _dbContext.UserServices.Where(a => a.ServiceId == serviceId).Select(a => a.UserId).ToList();

            }
            name = string.IsNullOrEmpty(name) ? string.Empty : name;
            var doctors = new List<UserProfile>();

            doctors = _dbContext.UserProfiles.Include(a => a.TimeClinicLocation)
                                                .ThenInclude(a => a.City)
                                                .Include(x => x.Certifications)
                                                .ThenInclude(a => a.Degree)
                                                .Include(a => a.PractitionerType)
                                                .Include(x => x.City)
                                                .Where(a => a.ProfileStatus == ProfileStatus.Active && a.AccountTypeId == 1
                                                 && (name == String.Empty || a.FullName.Contains(name))
                                                 && (serviceId == 0 || ids.Contains(a.Id))
                                                 && (cityId == 0 || a.TimeClinicLocation.CityId == cityId)
                                                 && (disctrictId == 0 || a.TimeClinicLocation.DistrictId == disctrictId))
                                                .ToList();


            if (insuranceType != 0)
            {
                doctors = doctors.Join(
                  _dbContext.UserCompanies,
                  doctor => doctor.Id,
                  userCompany => userCompany.UserProfileId,
                  (doctor, userCompany) => new { Doctor = doctor, UserCompany = userCompany })
                                        .Where(w => w.UserCompany.InsuranceCompanyId == insuranceType)
                                        .Select(s => s.Doctor)
                                        .ToList();
            }

            if (sortBy != 0)
            {
                if (sortBy == 1)
                    doctors = doctors.OrderBy(a => a.FullName).ToList();
                else
                {
                    doctors = doctors.OrderBy(a => a.TimeClinicLocation?.City?.TitleEn).ToList();
                }
            }

            return PartialView("DoctorsList", doctors);
        }
        public ActionResult GetDoctorDetails(string userId)
        {

            var doctorDetails = new DoctorDetailsModel();

            doctorDetails = _dbContext.UserProfiles.Where(w => w.UserId == userId)
                                                            .Include(i => i.Certifications)
                                                            .ThenInclude(cer => cer.Degree)
                                                             .Select(s => new DoctorDetailsModel
                                                             {
                                                                 UserProfileId = s.Id,
                                                                 UserId = s.UserId,
                                                                 FullName = s.FullName,
                                                                 ProfilePicturePath = s.ProfilePicturePath,
                                                                 InsuranceAccepted = s.InsuranceAccepted ?? false,
                                                                 OverView = new OverView()
                                                                 {
                                                                     Bio = s.Bio ?? "-",
                                                                     SpecialtiesTitlesAr = s.SpecialtiesTitlesAr ?? "-",
                                                                     SpecialtiesTitlesEn = s.SpecialtiesTitlesEn ?? "-",
                                                                     Certifications = s.Certifications
                                                                 }

                                                             }).Single();



            return PartialView("DoctorDetails", doctorDetails);
        }
        public ActionResult GetDoctorServices(int userProfileId)
        {
            List<UserServices> model = _dbContext.UserServices
                                                .Where(a => a.UserId == userProfileId && a.Status == Enums.ServicesStatusEnum.Approved)
                                                .ToList();

            return PartialView("DoctorServices", model);
        }
        public ActionResult GetTimeClinicLocations(int userProfileId)
        {
            string cultureCode = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            string direction = cultureCode.StartsWith("ar", StringComparison.OrdinalIgnoreCase) ? "rtl" : "ltr";
            bool isEng = direction == "ltr" ? true : false;


            var model = _dbContext.TimeClinicLocations.Include(i => i.State)
                                                      .Include(i => i.Country)
                                                      .Include(i => i.City)
                                                      .Include(i => i.Districts)
                                                      .SingleOrDefault(w => w.UserProfileId == userProfileId);


            if (model == null)
                model = new TimeClinicLocation();

            if (string.IsNullOrWhiteSpace(model?.ClinicName))
            {
                model.ClinicName = "Eihal Clinic";
            }

            if (isEng)
            {
                model.Location = string.Join("-",
                    model.Country?.TitleEn ?? "",
                    model.State?.TitleEn ?? "",
                    model.City?.TitleEn ?? "",
                    model.Districts?.TitleEn ?? "");
            }
            else
            {
                model.Location = string.Join("-",
                                    model.Country?.TitleAr ?? "",
                                    model.State?.TitleAr ??"",
                                    model.City?.TitleAr ?? "",
                                    model.Districts?.TitleAr ?? "");
            }


            if (string.IsNullOrWhiteSpace(model.SundayOpenAt))
            {
                model.SundayOpenAt = "00:00";
            }
            if (string.IsNullOrWhiteSpace(model.SundayClosedAt))
            {
                model.SundayClosedAt = "00:00";
            }

            if (string.IsNullOrWhiteSpace(model.MondayOpenAt))
            {
                model.MondayOpenAt = "00:00";
            }
            if (string.IsNullOrWhiteSpace(model.MondayClosedAt))
            {
                model.MondayClosedAt = "00:00";
            }

            if (string.IsNullOrWhiteSpace(model.TuesdayOpenAt))
            {
                model.TuesdayOpenAt = "00:00";
            }
            if (string.IsNullOrWhiteSpace(model.TuesdayClosedAt))
            {
                model.TuesdayClosedAt = "00:00";
            }

            if (string.IsNullOrWhiteSpace(model.WednesdayOpenAt))
            {
                model.WednesdayOpenAt = "00:00";
            }
            if (string.IsNullOrWhiteSpace(model.WednesdayClosedAt))
            {
                model.WednesdayClosedAt = "00:00";
            }

            if (string.IsNullOrWhiteSpace(model.ThursdayOpenAt))
            {
                model.ThursdayOpenAt = "00:00";
            }
            if (string.IsNullOrWhiteSpace(model.ThursdayClosedAt))
            {
                model.ThursdayClosedAt = "00:00";
            }

            if (string.IsNullOrWhiteSpace(model.FridayOpenAt))
            {
                model.FridayOpenAt = "00:00";
            }
            if (string.IsNullOrWhiteSpace(model.FridayClosedAt))
            {
                model.FridayClosedAt = "00:00";
            }

            if (string.IsNullOrWhiteSpace(model.SaturdayOpenAt))
            {
                model.SaturdayOpenAt = "00:00";
            }
            if (string.IsNullOrWhiteSpace(model.SaturdayClosedAt))
            {
                model.SaturdayClosedAt = "00:00";
            }

            return PartialView("DoctorTimeClinicLocations", model);
        }

        [HttpGet]
        public ActionResult GetUserCompanies(int userProfileId)
        {
            var result = _dbContext.UserCompanies
                                   .Include(i => i.Company)
                                   .Where(w => w.UserProfileId == userProfileId).Select(s => s.Company).ToList();
            return Json(result);
        }
        #endregion

        #region Culture
        [HttpGet]
        public IActionResult SetCulture(string culureCode, string returnUrl)
        {
            // Update the culture for the current request
            CultureInfo.CurrentCulture = new CultureInfo(culureCode);
            CultureInfo.CurrentUICulture = new CultureInfo(culureCode);

            // Store the selected culture in a persistent store (e.g., database, session, cookie)
            StoreCultureInCookies(culureCode);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        private void StoreCultureInCookies(string cultureCode)
        {
            // Store the selected culture in a cookie
            Response.Cookies.Append("CultureInfo", cultureCode);
        }
        #endregion

        #region Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}