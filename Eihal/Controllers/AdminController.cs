using Eihal.Data;
using Eihal.Data.Entites;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Eihal.Controllers
{

    [Route("Admin")]
    [Authorize(Roles = "Administrator")]
    public class AdminController : BaseController
    {

        public AdminController(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }


        #region
        [Route("MasterList/PractitionerTypes")]
        public IActionResult PractitionerTypes()
        {
            // Retrieve the value from TempData
            bool? isFromDeleteRequest = TempData["isFromDeleteRequest"] as bool?;
            bool? isSuccessDelete = TempData["isSuccessDelete"] as bool?;

            if (isFromDeleteRequest != null && isSuccessDelete != null)
            {
                // Clear the TempData value to avoid persisting it across subsequent requests
                TempData.Remove("isFromDeleteRequest");
                TempData.Remove("isSuccessDelete");

                // Use the value as needed
                ViewBag.SuccessMessage = isSuccessDelete;
            }

            return View(_dbContext.PractitionerTypes.ToList());
        }

        [Route("GetPractitionerTypes")]
        public IActionResult PractitionerTypesList()
        {
            var practitionerTypes = _dbContext.PractitionerTypes.ToList();


            return PartialView("ListPractitionerTypes", practitionerTypes);
        }

        [HttpPost]
        [Route("AddPractitionerType")]
        public IActionResult AddPractitionerType([FromBody] PractitionerType practitionerType)
        {
            if (practitionerType == null || string.IsNullOrEmpty(practitionerType.TitleEn) || string.IsNullOrEmpty(practitionerType.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.PractitionerTypes.Any(w => w.TitleEn == practitionerType.TitleEn || w.TitleAr == practitionerType.TitleAr);
            if (isDuplicate)
            {
                return BadRequest("The details for the Practitioner Type have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(practitionerType.TitleAr) || !string.IsNullOrEmpty(practitionerType.TitleEn))
                {
                    _dbContext.PractitionerTypes.Add(practitionerType);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpPost]
        [Route("UpdatePractitionerType")]
        public IActionResult UpdatePractitionerType([FromBody] PractitionerType practitionerType)
        {
            if (practitionerType == null || string.IsNullOrEmpty(practitionerType.TitleEn) || string.IsNullOrEmpty(practitionerType.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.PractitionerTypes.Any(w => w.TitleEn == practitionerType.TitleEn || w.TitleAr == practitionerType.TitleAr);
            if (isDuplicate)
            {
                return BadRequest("The details for the Practitioner Type have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(practitionerType.TitleAr) || !string.IsNullOrEmpty(practitionerType.TitleEn))
                {
                    _dbContext.PractitionerTypes.Update(practitionerType);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("DeletePractitionerType/{id}")]
        public IActionResult DeletePractitionerType(int id)
        {

            bool isLinked = _dbContext.ApplicationUsers.Any(w => w.PractitionerTypeId == id);
            if (isLinked)
            {
                return BadRequest("You cannot delete this item as it is linked to users in the system.");
            }

            else
            {
                // Retrieve the practitioner type from the database using the id
                var practitionerType = _dbContext.PractitionerTypes.Find(id);


                if (practitionerType == null)
                {
                    // Handle the case where the practitioner type doesn't exist
                    TempData["isSuccessDelete"] = false;
                }

                // Remove the practitioner type from the DbSet
                _dbContext.PractitionerTypes.Remove(practitionerType);

                // Save the changes to the database
                _dbContext.SaveChanges();
                TempData["isSuccessDelete"] = true;
            }
            // Set the value in TempData
            TempData["isFromDeleteRequest"] = true;



            return RedirectToAction("PractitionerTypes");
        }


        [HttpGet]
        [Route("GetPractitionerType/{id}")]
        public PractitionerType GetPractitionerType(int id)
        {
            var practitionerTypes = _dbContext.PractitionerTypes.Single(w => w.Id == id);
            return practitionerTypes;
        }


        #endregion

        #region
        [Route("MasterList/ProfessionalRank")]
        public IActionResult ProfessionalRank()
        {
            return View();
        }
        #endregion


    }
}
