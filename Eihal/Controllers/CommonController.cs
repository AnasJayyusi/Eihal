using Eihal.Data;
using Eihal.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace Eihal.Controllers
{

    [Route("Common")]
    public class CommonController : BaseController
    {
        public CommonController(ApplicationDbContext dbContext, INotificationService notificationService) : base(dbContext, notificationService)
        {
        }

        #region Shared DropDownList

        [HttpGet]
        [Route("GetAccountTypesDDL")]
        public ActionResult GetAccountTypesDDL()
        {
            // Retrieve the data for the dropdown list
            var dropdownData = _dbContext.AccountTypes.ToList();

            // Pass the data to the view
            return Json(dropdownData);
        }

        [HttpGet]
        [Route("GetPractitionerTypesDDL")]
        public ActionResult GetPractitionerTypesDDL()
        {
            // Retrieve the data for the dropdown list
            var dropdownData = _dbContext.PractitionerTypes.Where(w => w.IsActive).ToList();

            // Pass the data to the view
            return Json(dropdownData);
        }

        [HttpGet]
        [Route("GetClinicalSpecialitiesDDL")]
        public ActionResult GetClinicalSpecialitiesDDL()
        {
            // Retrieve the data for the dropdown list
            var dropdownData = _dbContext.ClinicalSpecialities.Where(w => w.IsActive).ToList();

            // Pass the data to the view
            return Json(dropdownData);
        }
        [HttpGet]
        [Route("GetSubPrivillagesDDL")]
        public ActionResult GetSubPrivillagesDDL(int? privillageId)
        {
            // Retrieve the data for the dropdown list
            var dropdownData = _dbContext.SubPrivillages.Where(w => (privillageId == null || w.PrivillageId == privillageId) && w.IsActive).ToList();

            // Pass the data to the view
            return Json(dropdownData);
        }
        [HttpGet]
        [Route("GetPrivillagesDDL")]
        public ActionResult GetPrivillagesDDL()
        {
            // Retrieve the data for the dropdown list
            var dropdownData = _dbContext.Privillages.Where(w => w.IsActive).ToList();

            // Pass the data to the view
            return Json(dropdownData);
        }
        [HttpGet]
        [Route("GetSpecialitiesDDL")]
        public ActionResult GetSpecialitiesDDL()
        {
            // Retrieve the data for the dropdown list
            var dropdownData = _dbContext.Specialties.Where(w => w.IsActive).ToList();

            // Pass the data to the view
            return Json(dropdownData);
        }

        [HttpGet]
        [Route("GetCountriesDDL")]
        public ActionResult GetCountriesDDL()
        {
            // Retrieve the data for the dropdown list
            var dropdownData = _dbContext.Countries.ToList();

            // Pass the data to the view
            return Json(dropdownData);
        }
        [HttpGet]
        [Route("GetServicesDDL")]
        public ActionResult GetServicesDDL()
        {
            // Retrieve the data for the dropdown list
            var dropdownData = _dbContext.Services.ToList();

            // Pass the data to the view
            return Json(dropdownData);
        }

        [HttpGet]
        [Route("GetStatesDDL")]
        public ActionResult GetStatesDDL()
        {
            // Retrieve the data for the dropdown list
            var dropdownData = _dbContext.States.ToList();

            // Pass the data to the view
            return Json(dropdownData);
        }

        [HttpGet]
        [Route("GetCitiesDDL")]
        public ActionResult GetCitiesDDL()
        {
            // Retrieve the data for the dropdown list
            var dropdownData = _dbContext.Cities.ToList();

            // Pass the data to the view
            return Json(dropdownData);
        }

        [HttpGet]
        [Route("GetDistrictsDDL")]
        public ActionResult GetDisctrictsDDL(int cityId)
        {
            // Retrieve the data for the dropdown list
            var dropdownData = _dbContext.Districts.Where(a=>a.CityId == cityId).ToList();

            // Pass the data to the view
            return Json(dropdownData);
        }
        [HttpGet]
        [Route("GetInsuranceCompaniesDDL")]
        public ActionResult GetInsuranceCompaniesDDL()
        {
            // Retrieve the data for the dropdown list
            var dropdownData = _dbContext.InsuranceCompanies.Where(a=>a.IsActive).ToList();

            // Pass the data to the view
            return Json(dropdownData);
        }

      
        #endregion

        #region Helper For Razor Page

        [AllowAnonymous]
        [HttpGet]
        [Route("CheckPhoneNumber")]
        public bool CheckPhoneNumber(string phoneNumber)
        {
            bool isUsed = false;

            var userProfile = _dbContext.UserProfiles.FirstOrDefault(x => x.PhoneNumber.Equals(phoneNumber));
            if (userProfile != null)
            {
                isUsed = true;
            }

            return isUsed;
        }
        #endregion

        #region Select2 AutoComplete DDLS
        [HttpGet]
        [Route("GetInsuranceCompanies")]
        public ActionResult GetInsuranceCompanies(string term)
        {
            var results = _dbContext.InsuranceCompanies
                             .Where(w => w.IsActive)
                             .Select(s => new { id = s.Id, text = s.TitleEn, textAr = s.TitleAr })
                             .Where(x => string.IsNullOrEmpty(term) || x.text.Contains(term) || x.textAr.Contains(term))
                             .ToList();

            return Ok(new { results });
        }


        [HttpGet]
        [Route("GetDoctorsProvider")]
        public ActionResult GetDoctorsProvider(string term)
        {
            var query = from users in _dbContext.UserProfiles
                        join userService in _dbContext.UserServices on users.Id equals userService.UserId
                        select new
                        {
                            id = users.Id,
                            text = userService.TitleEn,
                            textAr = userService.TitleAr
                        };

            var result = query.Where(x => string.IsNullOrEmpty(term) || x.text.Contains(term) || x.textAr.Contains(term)).ToList();

            return Ok(new { result });
        }


        [HttpGet]
        [Route("GetAllServicesDLL")]
        public ActionResult GetAllServicesDLL(string term)
        {
            var query = from services in _dbContext.Services
                        join userService in _dbContext.UserServices on services.Id equals userService.ServiceId
                        select new
                        {
                           id= services.Id,
                           text= services.TitleEn,
                           textAr= services.TitleAr
                        };

            var result = query.Where(x => string.IsNullOrEmpty(term) || x.text.Contains(term) || x.textAr.Contains(term)).ToList();

            return Ok(new { result });
        }

        [HttpGet]
        [Route("GetUserSpecialtiesDDL")]
        public ActionResult GetUserSpecialtiesDDL(string term)
        {
            _loggedAspNetUserId = GetAspNetUserId();
            var practitionerTypeId = _dbContext.UserProfiles
                                                .Where(u => u.UserId == GetAspNetUserId())
                                                .Select(u => u.PractitionerTypeId);

            var results = _dbContext.Specialties
                             .Where(w => w.IsActive && practitionerTypeId.Contains(w.PractitionerTypeId))
                             .Select(s => new { id = s.Id, text = s.TitleEn, textAr = s.TitleAr })
                             .Where(x => string.IsNullOrEmpty(term) || x.text.Contains(term) || x.textAr.Contains(term))
                             .ToList();

            return Ok(new { results });
        }
        #endregion
    }
}
