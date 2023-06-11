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
    }
}
