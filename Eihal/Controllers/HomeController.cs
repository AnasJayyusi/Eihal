using Eihal.Data;
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

        public HomeController(ApplicationDbContext dbContext, ILogger<HomeController> logger) : base(dbContext)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            if (User.IsInRole(UserRolesEnum.Beneficiary.ToString()))
            {
                return RedirectToAction("Profile", "ServiceProvider");
            }
            if (User.IsInRole(UserRolesEnum.ServiceProvider.ToString()))
            {
                return RedirectToAction("Profile", "ServiceProvider");
            }
            if (User.IsInRole(UserRolesEnum.Administrator.ToString()))
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            return View();
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
        public IActionResult Doctors()
        {
            var doctors = _dbContext.UserProfiles.Include(x => x.Certifications).ThenInclude(a => a.Degree).Include(a=> a.PractitionerType).Include(a=>a.City).Where(a => a.AccountTypeId == 1).ToList();
            return View(doctors);
        }
        public ActionResult FillDoctorsList(string name,int serviceId,int cityId,int disctrictId,int sortBy,int insuranceType)
        {
            List<int> ids = new List<int>();
            if (serviceId > 0)
            {
                ids = _dbContext.UserServices.Where(a => a.ServiceId == serviceId).Select(a => a.UserId).ToList();

            }
            name = string.IsNullOrEmpty(name) ? string.Empty : name;
            var doctors = _dbContext.UserProfiles.Include(x=>x.Certifications).ThenInclude(a=>a.Degree).Include(a => a.PractitionerType).Include(x=>x.City).Where(a => a.AccountTypeId == 1 
           && (name == String.Empty || a.FullName.Contains(name))
            && (serviceId == 0  || ids.Contains( a.Id))
            && (cityId ==0 || a.CityId == cityId) 
            && (insuranceType == 0 || (insuranceType == 1 && a.InsuranceAccepted == true) || (insuranceType == 2 && a.InsuranceAccepted != true))
           
            ).ToList();
            if (sortBy != 0) {
                if(sortBy == 1)
                doctors = doctors.OrderBy(a => a.FullName).ToList();
            else
                {
                    doctors = doctors.OrderBy(a => a.City).ToList();
                }
            }
            return PartialView("_DoctorsList",doctors);
        }

        public ActionResult GetUserDetailsById(string userId)
        {
            var userDetails = _dbContext.UserProfiles.Include(x => x.Certifications).ThenInclude(a => a.Degree).Include(a => a.PractitionerType).Include(x => x.City).Where(a => a.UserId == userId
            ).FirstOrDefault();
          
            return PartialView("_UserDetails", userDetails);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult AdminOnly()
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