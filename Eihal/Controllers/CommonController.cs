using Eihal.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
namespace Eihal.Controllers
{

    [Route("Common")]
    [Authorize(Roles = "Administrator,ServiceProvider")]
    public class CommonController : BaseController
    {
        public CommonController(ApplicationDbContext dbContext) : base(dbContext)
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

        //[HttpGet]
        //public ActionResult GetUserSpecialties(string term)
        //{
        //    var results = new List<object>
        //    {
        //        new { id = 1, text = "Option 1" },
        //        new { id = 2, text = "Option 2", selected = true },
        //        new { id = 3, text = "Option 3", disabled = true }
        //    };

        //    results.Where(x => x.text == term);
        //    return Ok(new { });
        //}

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
    }
}
