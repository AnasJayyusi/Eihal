using Eihal.Data;
using Eihal.Data.Entites;
using Eihal.Hubs;
using Eihal.Models;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Contact()
        {
            return View();
        }
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
            var userDetails = _dbContext.UserProfiles.Include(x => x.Certifications)
                                                     .ThenInclude(a => a.Degree)
                                                     .Include(a => a.PractitionerType)
                                                     .Include(x => x.City).Where(a => a.UserId == userId).FirstOrDefault();

            return PartialView("DoctorDetails", userDetails);
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}