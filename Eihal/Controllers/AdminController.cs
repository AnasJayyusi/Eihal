using Eihal.Data;
using Eihal.Data.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eihal.Controllers
{

    [Route("Admin")]
    [Authorize(Roles = "Administrator")]
    public class AdminController : BaseController
    {

        public AdminController(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        #region Dashboard
        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }
        #endregion

        #region PractitionerTypes
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

        #region ProfessionalRanks
        [Route("MasterList/ProfessionalRanks")]
        public IActionResult ProfessionalRanks()
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

            return View(_dbContext.ProfessionalRank.ToList());
        }


        [Route("GetProfessionalRanks")]
        public IActionResult ProfessionalRanksList()
        {
            var professionalRanks = _dbContext.ProfessionalRank.ToList();
            return PartialView("ListProfessionalRanks", professionalRanks);
        }

        [HttpPost]
        [Route("AddProfessionalRank")]
        public IActionResult AddProfessionalRank([FromBody] ProfessionalRank professionalRank)
        {
            if (professionalRank == null || string.IsNullOrEmpty(professionalRank.TitleEn) || string.IsNullOrEmpty(professionalRank.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.ProfessionalRank.Any(w => w.TitleEn == professionalRank.TitleEn || w.TitleAr == professionalRank.TitleAr);
            if (isDuplicate)
            {
                return BadRequest("The details for the Professional Rank have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(professionalRank.TitleAr) || !string.IsNullOrEmpty(professionalRank.TitleEn))
                {
                    _dbContext.ProfessionalRank.Add(professionalRank);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpPost]
        [Route("UpdateProfessionalRank")]
        public IActionResult UpdateProfessionalRank([FromBody] ProfessionalRank professionalRank)
        {
            if (professionalRank == null || string.IsNullOrEmpty(professionalRank.TitleEn) || string.IsNullOrEmpty(professionalRank.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.ProfessionalRank.Any(w => w.TitleEn == professionalRank.TitleEn || w.TitleAr == professionalRank.TitleAr);
            if (isDuplicate)
            {
                return BadRequest("The details for the Professional Rank have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(professionalRank.TitleAr) || !string.IsNullOrEmpty(professionalRank.TitleEn))
                {
                    _dbContext.ProfessionalRank.Update(professionalRank);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("DeleteProfessionalRank/{id}")]
        public IActionResult DeleteProfessionalRank(int id)
        {

            bool isLinked = _dbContext.ApplicationUsers.Any(w => w.ProfessionalRankId == id);
            if (isLinked)
            {
                return BadRequest("You cannot delete this item as it is linked to users in the system.");
            }

            else
            {
                // Retrieve the practitioner type from the database using the id
                var professionalRank = _dbContext.ProfessionalRank.Find(id);


                if (professionalRank == null)
                {
                    // Handle the case where the practitioner type doesn't exist
                    TempData["isSuccessDelete"] = false;
                }

                // Remove the practitioner type from the DbSet
                _dbContext.ProfessionalRank.Remove(professionalRank);

                // Save the changes to the database
                _dbContext.SaveChanges();
                TempData["isSuccessDelete"] = true;
            }
            // Set the value in TempData
            TempData["isFromDeleteRequest"] = true;
            return RedirectToAction("ProfessionalRanks");
        }


        [HttpGet]
        [Route("GetProfessionalRank/{id}")]
        public ProfessionalRank GetProfessionalRank(int id)
        {
            var professionalRanks = _dbContext.ProfessionalRank.Single(w => w.Id == id);
            return professionalRanks;
        }
        #endregion
    }
}
