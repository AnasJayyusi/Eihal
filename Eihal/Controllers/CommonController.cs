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
