﻿using Eihal.Data;
using Eihal.Data.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eihal.Controllers
{

    [Route("Admin")]
    [Authorize(Roles = "Administrator")]
    public class AdminController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment) : base(dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        #region Dashboard
        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            var services = _dbContext.UserServices.Where(a => a.Status == Enums.ServicesStatusEnum.Pending).Include(a => a.UserProfile).ToList();
            return View(services);
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
            return PartialView("PractitionerTypesList", practitionerTypes);
        }

        [HttpPost]
        [Route("AddPractitionerType")]
        public IActionResult AddPractitionerType([FromBody] PractitionerType practitionerType)
        {
            if (practitionerType == null || string.IsNullOrEmpty(practitionerType.TitleEn) || string.IsNullOrEmpty(practitionerType.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.PractitionerTypes.Any(w => w.TitleEn == practitionerType.TitleEn && w.TitleAr == practitionerType.TitleAr);
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

            bool isDuplicate = _dbContext.PractitionerTypes.Any(w => w.TitleEn == practitionerType.TitleEn && w.TitleAr == practitionerType.TitleAr);
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
        [Route("UpdatePractitionerTypeStatus/{id}/{isActive}")]
        public IActionResult UpdatePractitionerTypeStatus(int id, bool isActive)
        {
            // Logic to update the status of the practitioner type with the given ID
            try
            {
                var practitionerType = _dbContext.PractitionerTypes.SingleOrDefault(p => p.Id == id);
                if (practitionerType == null)
                {
                    return NotFound();
                }

                practitionerType.IsActive = isActive;
                _dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the status.");
            }
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

            return View(_dbContext.ProfessionalRanks.Include(i => i.PractitionerType).ToList());
        }


        [Route("GetProfessionalRanks")]
        public IActionResult ProfessionalRanksList()
        {
            var professionalRanks = _dbContext.ProfessionalRanks.Include(i => i.PractitionerType).ToList();
            return PartialView("ProfessionalRanksList", professionalRanks);
        }

        [HttpPost]
        [Route("AddProfessionalRank")]
        public IActionResult AddProfessionalRank([FromBody] ProfessionalRank professionalRank)
        {
            if (professionalRank == null || string.IsNullOrEmpty(professionalRank.TitleEn) || string.IsNullOrEmpty(professionalRank.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.ProfessionalRanks
                                         .Any(w => (w.TitleEn == professionalRank.TitleEn && w.TitleAr == professionalRank.TitleAr)
                                         && w.PractitionerTypeId == professionalRank.PractitionerTypeId);
            if (isDuplicate)
            {
                return BadRequest("The details for the Professional Rank have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(professionalRank.TitleAr) || !string.IsNullOrEmpty(professionalRank.TitleEn))
                {
                    _dbContext.ProfessionalRanks.Add(professionalRank);
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

            bool isDuplicate = _dbContext.ProfessionalRanks
                                        .Any(w => (w.TitleEn == professionalRank.TitleEn && w.TitleAr == professionalRank.TitleAr)
                                        && w.PractitionerTypeId == professionalRank.PractitionerTypeId);
            if (isDuplicate)
            {
                return BadRequest("The details for the Professional Rank have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(professionalRank.TitleAr) || !string.IsNullOrEmpty(professionalRank.TitleEn))
                {
                    _dbContext.ProfessionalRanks.Update(professionalRank);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("UpdateProfessionalRankStatus/{id}/{isActive}")]
        public IActionResult UpdateProfessionalRankStatus(int id, bool isActive)
        {
            // Logic to update the status of the practitioner type with the given ID
            try
            {
                var professionalRank = _dbContext.ProfessionalRanks.SingleOrDefault(p => p.Id == id);
                if (professionalRank == null)
                {
                    return NotFound();
                }

                professionalRank.IsActive = isActive;
                _dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the status.");
            }
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
                var professionalRank = _dbContext.ProfessionalRanks.Find(id);


                if (professionalRank == null)
                {
                    // Handle the case where the practitioner type doesn't exist
                    TempData["isSuccessDelete"] = false;
                }

                // Remove the practitioner type from the DbSet
                _dbContext.ProfessionalRanks.Remove(professionalRank);

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
            var professionalRanks = _dbContext.ProfessionalRanks.Single(w => w.Id == id);
            return professionalRanks;
        }
        #endregion

        #region Specialties
        [Route("MasterList/Specialties")]
        public IActionResult Specialties()
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

            return View(_dbContext.Specialties.Include(i => i.PractitionerType).ToList());
        }


        [Route("GetSpecialties")]
        public IActionResult SpecialtiesList()
        {
            var specialties = _dbContext.Specialties.Include(i => i.PractitionerType).ToList();
            return PartialView("SpecialtiesList", specialties);
        }

        [HttpPost]
        [Route("AddSpecialty")]
        public IActionResult AddSpecialty([FromBody] Specialty specialty)
        {
            if (specialty == null || string.IsNullOrEmpty(specialty.TitleEn) || string.IsNullOrEmpty(specialty.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.Specialties
                                         .Any(w => (w.TitleEn == specialty.TitleEn && w.TitleAr == specialty.TitleAr)
                                         && w.PractitionerTypeId == specialty.PractitionerTypeId);
            if (isDuplicate)
            {
                return BadRequest("The details for the Specialty have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(specialty.TitleAr) || !string.IsNullOrEmpty(specialty.TitleEn))
                {
                    _dbContext.Specialties.Add(specialty);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpPost]
        [Route("UpdateSpecialty")]
        public IActionResult UpdateSpecialty([FromBody] Specialty specialty)
        {
            if (specialty == null || string.IsNullOrEmpty(specialty.TitleEn) || string.IsNullOrEmpty(specialty.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.Specialties
                                        .Any(w => (w.TitleEn == specialty.TitleEn && w.TitleAr == specialty.TitleAr)
                                        && w.PractitionerTypeId == specialty.PractitionerTypeId);
            if (isDuplicate)
            {
                return BadRequest("The details for the Specialty have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(specialty.TitleAr) || !string.IsNullOrEmpty(specialty.TitleEn))
                {
                    _dbContext.Specialties.Update(specialty);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("UpdateSpecialtyStatus/{id}/{isActive}")]
        public IActionResult UpdateSpecialtyStatus(int id, bool isActive)
        {
            // Logic to update the status of the practitioner type with the given ID
            try
            {
                var specialty = _dbContext.Specialties.SingleOrDefault(p => p.Id == id);
                if (specialty == null)
                {
                    return NotFound();
                }

                specialty.IsActive = isActive;
                _dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the status.");
            }
        }

        [HttpGet]
        [Route("DeleteSpecialty/{id}")]
        public IActionResult DeleteSpecialty(int id)
        {
            // Retrieve the practitioner type from the database using the id
            var specialty = _dbContext.Specialties.Find(id);

            if (specialty == null)
            {
                // Handle the case where the practitioner type doesn't exist
                TempData["isSuccessDelete"] = false;
            }

            // Remove the practitioner type from the DbSet
            _dbContext.Specialties.Remove(specialty);

            // Save the changes to the database
            _dbContext.SaveChanges();
            TempData["isSuccessDelete"] = true;
            // Set the value in TempData
            TempData["isFromDeleteRequest"] = true;
            return RedirectToAction("Specialties");
        }


        [HttpGet]
        [Route("GetSpecialty/{id}")]
        public Specialty GetSpecialty(int id)
        {
            var specialty = _dbContext.Specialties.Single(w => w.Id == id);
            return specialty;
        }
        #endregion

        #region Subspecialties
        [Route("MasterList/Subspecialties")]
        public IActionResult Subspecialties()
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

            return View(_dbContext.Subspecialty.Include(i => i.Specialty).ToList());
        }


        [Route("GetSubspecialties")]
        public IActionResult SubspecialtiesList()
        {
            var subspecialties = _dbContext.Subspecialty.Include(i => i.Specialty).ToList();
            return PartialView("SubspecialtiesList", subspecialties);
        }

        [HttpPost]
        [Route("AddSubspeciality")]
        public IActionResult AddSubspeciality([FromBody] Subspecialty subspecialty)
        {
            if (subspecialty == null || string.IsNullOrEmpty(subspecialty.TitleEn) || string.IsNullOrEmpty(subspecialty.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.Subspecialty
                                         .Any(w => (w.TitleEn == subspecialty.TitleEn && w.TitleAr == subspecialty.TitleAr)
                                         && w.SpecialityId == subspecialty.SpecialityId);
            if (isDuplicate)
            {
                return BadRequest("The details for the Subspecialty have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(subspecialty.TitleAr) || !string.IsNullOrEmpty(subspecialty.TitleEn))
                {
                    _dbContext.Subspecialty.Add(subspecialty);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpPost]
        [Route("UpdateSubspeciality")]
        public IActionResult UpdateSubspeciality([FromBody] Subspecialty subspecialty)
        {
            if (subspecialty == null || string.IsNullOrEmpty(subspecialty.TitleEn) || string.IsNullOrEmpty(subspecialty.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.Subspecialty
                                        .Any(w => (w.TitleEn == subspecialty.TitleEn && w.TitleAr == subspecialty.TitleAr)
                                        && w.SpecialityId == subspecialty.SpecialityId);
            if (isDuplicate)
            {
                return BadRequest("The details for the subspecialty have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(subspecialty.TitleAr) || !string.IsNullOrEmpty(subspecialty.TitleEn))
                {
                    _dbContext.Subspecialty.Update(subspecialty);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("UpdateSubspecialityStatus/{id}/{isActive}")]
        public IActionResult UpdateSubspecialityStatus(int id, bool isActive)
        {
            // Logic to update the status of the practitioner type with the given ID
            try
            {
                var subspecialty = _dbContext.Subspecialty.SingleOrDefault(p => p.Id == id);
                if (subspecialty == null)
                {
                    return NotFound();
                }

                subspecialty.IsActive = isActive;
                _dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the status.");
            }
        }

        [HttpGet]
        [Route("DeleteSubspeciality/{id}")]
        public IActionResult DeleteSubspeciality(int id)
        {
            // Retrieve the practitioner type from the database using the id
            var subspecialty = _dbContext.Subspecialty.Find(id);

            if (subspecialty == null)
            {
                // Handle the case where the practitioner type doesn't exist
                TempData["isSuccessDelete"] = false;
            }

            // Remove the practitioner type from the DbSet
            _dbContext.Subspecialty.Remove(subspecialty);

            // Save the changes to the database
            _dbContext.SaveChanges();
            TempData["isSuccessDelete"] = true;
            // Set the value in TempData
            TempData["isFromDeleteRequest"] = true;
            return RedirectToAction("Subspecialties");
        }


        [HttpGet]
        [Route("GetSubspeciality/{id}")]
        public Subspecialty GetSubspeciality(int id)
        {
            var subspecialty = _dbContext.Subspecialty.Single(w => w.Id == id);
            return subspecialty;
        }

        [Route("GetSubspecialities")]
        public IActionResult GetSubspecialitiesList()
        {
            var subspecialty = _dbContext.Subspecialty.Include(i => i.Specialty).ToList();
            return PartialView("SubspecialitiesList", subspecialty);
        }
        #endregion

        #region ClinicalSpecialities
        [Route("MasterList/ClinicalSpecialities")]
        public IActionResult ClinicalSpecialities()
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

            return View(_dbContext.ClinicalSpecialities.Include(i => i.PractitionerType).ToList());
        }


        [Route("GetClinicalSpecialities")]
        public IActionResult ClinicalSpecialitiesList()
        {
            var clinicalSpecialities = _dbContext.ClinicalSpecialities.Include(i => i.PractitionerType).ToList();
            return PartialView("ClinicalSpecialitiesList", clinicalSpecialities);
        }

        [HttpPost]
        [Route("AddClinicalSpeciality")]
        public IActionResult AddClinicalSpeciality([FromBody] ClinicalSpeciality clinicalSpeciality)
        {
            if (clinicalSpeciality == null || string.IsNullOrEmpty(clinicalSpeciality.TitleEn) || string.IsNullOrEmpty(clinicalSpeciality.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.ClinicalSpecialities
                                         .Any(w => (w.TitleEn == clinicalSpeciality.TitleEn && w.TitleAr == clinicalSpeciality.TitleAr)
                                         && w.PractitionerTypeId == clinicalSpeciality.PractitionerTypeId);
            if (isDuplicate)
            {
                return BadRequest("The details for the Subspecialty have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(clinicalSpeciality.TitleAr) || !string.IsNullOrEmpty(clinicalSpeciality.TitleEn))
                {
                    _dbContext.ClinicalSpecialities.Add(clinicalSpeciality);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpPost]
        [Route("UpdateClinicalSpeciality")]
        public IActionResult UpdateClinicalSpeciality([FromBody] ClinicalSpeciality clinicalSpeciality)
        {
            if (clinicalSpeciality == null || string.IsNullOrEmpty(clinicalSpeciality.TitleEn) || string.IsNullOrEmpty(clinicalSpeciality.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.ClinicalSpecialities
                                        .Any(w => (w.TitleEn == clinicalSpeciality.TitleEn && w.TitleAr == clinicalSpeciality.TitleAr)
                                        && w.PractitionerTypeId == clinicalSpeciality.PractitionerTypeId);
            if (isDuplicate)
            {
                return BadRequest("The details for the subspecialty have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(clinicalSpeciality.TitleAr) || !string.IsNullOrEmpty(clinicalSpeciality.TitleEn))
                {
                    _dbContext.ClinicalSpecialities.Update(clinicalSpeciality);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("UpdateClinicalSpecialityStatus/{id}/{isActive}")]
        public IActionResult UpdateClinicalSpecialityStatus(int id, bool isActive)
        {
            // Logic to update the status of the practitioner type with the given ID
            try
            {
                var clinicalSpeciality = _dbContext.ClinicalSpecialities.SingleOrDefault(p => p.Id == id);
                if (clinicalSpeciality == null)
                {
                    return NotFound();
                }

                clinicalSpeciality.IsActive = isActive;
                _dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the status.");
            }
        }

        [HttpGet]
        [Route("DeleteClinicalSpeciality/{id}")]
        public IActionResult DeleteClinicalSpeciality(int id)
        {
            // Retrieve the practitioner type from the database using the id
            var clinicalSpeciality = _dbContext.ClinicalSpecialities.Find(id);

            if (clinicalSpeciality == null)
            {
                // Handle the case where the practitioner type doesn't exist
                TempData["isSuccessDelete"] = false;
            }

            // Remove the practitioner type from the DbSet
            _dbContext.ClinicalSpecialities.Remove(clinicalSpeciality);

            // Save the changes to the database
            _dbContext.SaveChanges();
            TempData["isSuccessDelete"] = true;
            // Set the value in TempData
            TempData["isFromDeleteRequest"] = true;
            return RedirectToAction("ClinicalSpecialities");
        }


        [HttpGet]
        [Route("GetClinicalSpeciality/{id}")]
        public ClinicalSpeciality GetClinicalSpeciality(int id)
        {
            var clinicalSpeciality = _dbContext.ClinicalSpecialities.Single(w => w.Id == id);
            return clinicalSpeciality;
        }

        [Route("GetClinicalSpecialitiesList")]
        public IActionResult GetClinicalSpecialitiesList()
        {
            var subspecialty = _dbContext.Subspecialty.Include(i => i.Specialty).ToList();
            return PartialView("SubspecialitiesList", subspecialty);
        }
        #endregion

        #region Privillages
        [Route("MasterList/Privillages")]
        public IActionResult Privillages()
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

            return View(_dbContext.Privillages.Include(i => i.ClinicalSpeciality).ToList());
        }


        [Route("GetPrivillages")]
        public IActionResult PrivillagesList()
        {
            var privillages = _dbContext.Privillages.Include(i => i.ClinicalSpeciality).ToList();
            return PartialView("PrivillagesList", privillages);
        }

        [HttpPost]
        [Route("AddPrivillage")]
        public IActionResult AddPrivillage([FromBody] Privillage privillage)
        {
            if (privillage == null || string.IsNullOrEmpty(privillage.TitleEn) || string.IsNullOrEmpty(privillage.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.Privillages
                                         .Any(w => (w.TitleEn == privillage.TitleEn && w.TitleAr == privillage.TitleAr)
                                         && w.ClinicalSpecialityId == privillage.ClinicalSpecialityId);
            if (isDuplicate)
            {
                return BadRequest("The details for the Subspecialty have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(privillage.TitleAr) || !string.IsNullOrEmpty(privillage.TitleEn))
                {
                    _dbContext.Privillages.Add(privillage);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpPost]
        [Route("UpdatePrivillage")]
        public IActionResult UpdatePrivillage([FromBody] Privillage privillage)
        {
            if (privillage == null || string.IsNullOrEmpty(privillage.TitleEn) || string.IsNullOrEmpty(privillage.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.Privillages
                                        .Any(w => (w.TitleEn == privillage.TitleEn && w.TitleAr == privillage.TitleAr)
                                        && w.ClinicalSpecialityId == privillage.ClinicalSpecialityId);
            if (isDuplicate)
            {
                return BadRequest("The details for the subspecialty have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(privillage.TitleAr) || !string.IsNullOrEmpty(privillage.TitleEn))
                {
                    _dbContext.Privillages.Update(privillage);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("UpdatePrivillageStatus/{id}/{isActive}")]
        public IActionResult UpdatePrivillageStatus(int id, bool isActive)
        {
            // Logic to update the status of the practitioner type with the given ID
            try
            {
                var privillage = _dbContext.Privillages.SingleOrDefault(p => p.Id == id);
                if (privillage == null)
                {
                    return NotFound();
                }

                privillage.IsActive = isActive;
                _dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the status.");
            }
        }

        [HttpGet]
        [Route("DeletePrivillage/{id}")]
        public IActionResult DeletePrivillage(int id)
        {
            // Retrieve the practitioner type from the database using the id
            var privillages = _dbContext.Privillages.Find(id);

            if (privillages == null)
            {
                // Handle the case where the practitioner type doesn't exist
                TempData["isSuccessDelete"] = false;
            }

            // Remove the practitioner type from the DbSet
            _dbContext.Privillages.Remove(privillages);

            // Save the changes to the database
            _dbContext.SaveChanges();
            TempData["isSuccessDelete"] = true;
            // Set the value in TempData
            TempData["isFromDeleteRequest"] = true;
            return RedirectToAction("Privillages");
        }


        [HttpGet]
        [Route("GetPrivillage/{id}")]
        public Privillage GetPrivillage(int id)
        {
            var privillages = _dbContext.Privillages.Single(w => w.Id == id);
            return privillages;
        }

        [Route("GetPrivillagesList")]
        public IActionResult GetPrivillagesList()
        {
            var privillages = _dbContext.Privillages.Include(i => i.ClinicalSpeciality).ToList();
            return PartialView("Privillages", privillages);
        }
        #endregion

        #region SubPrivillages
        [Route("MasterList/SubPrivillages")]
        public IActionResult SubPrivillages()
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

            return View(_dbContext.SubPrivillages.Include(i => i.Privillage).ToList());
        }


        [Route("GetSubPrivillages")]
        public IActionResult SubPrivillagesList()
        {
            var subPrivillages = _dbContext.SubPrivillages.Include(i => i.Privillage).ToList();
            return PartialView("SubPrivillagesList", subPrivillages);
        }

        [HttpPost]
        [Route("AddSubPrivillage")]
        public IActionResult AddSubPrivillage([FromBody] SubPrivillage subPrivillage)
        {
            if (subPrivillage == null || string.IsNullOrEmpty(subPrivillage.TitleEn) || string.IsNullOrEmpty(subPrivillage.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.SubPrivillages
                                         .Any(w => (w.TitleEn == subPrivillage.TitleEn && w.TitleAr == subPrivillage.TitleAr)
                                         && w.PrivillageId == subPrivillage.PrivillageId);
            if (isDuplicate)
            {
                return BadRequest("The details for the Subspecialty have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(subPrivillage.TitleAr) || !string.IsNullOrEmpty(subPrivillage.TitleEn))
                {
                    _dbContext.SubPrivillages.Add(subPrivillage);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpPost]
        [Route("UpdateSubPrivillage")]
        public IActionResult UpdateSubPrivillage([FromBody] SubPrivillage subPrivillage)
        {
            if (subPrivillage == null || string.IsNullOrEmpty(subPrivillage.TitleEn) || string.IsNullOrEmpty(subPrivillage.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.SubPrivillages
                                        .Any(w => (w.TitleEn == subPrivillage.TitleEn && w.TitleAr == subPrivillage.TitleAr)
                                        && w.PrivillageId == subPrivillage.PrivillageId);
            if (isDuplicate)
            {
                return BadRequest("The details for the sub privillage have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(subPrivillage.TitleAr) || !string.IsNullOrEmpty(subPrivillage.TitleEn))
                {
                    _dbContext.SubPrivillages.Update(subPrivillage);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("UpdateSubPrivillageStatus/{id}/{isActive}")]
        public IActionResult UpdateSubPrivillageStatus(int id, bool isActive)
        {
            // Logic to update the status of the practitioner type with the given ID
            try
            {
                var subPrivillage = _dbContext.SubPrivillages.SingleOrDefault(p => p.Id == id);
                if (subPrivillage == null)
                {
                    return NotFound();
                }

                subPrivillage.IsActive = isActive;
                _dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the status.");
            }
        }

        [HttpGet]
        [Route("DeleteSubPrivillage/{id}")]
        public IActionResult DeleteSubPrivillage(int id)
        {
            // Retrieve the practitioner type from the database using the id
            var subPrivillages = _dbContext.SubPrivillages.Find(id);

            if (subPrivillages == null)
            {
                // Handle the case where the practitioner type doesn't exist
                TempData["isSuccessDelete"] = false;
            }

            // Remove the practitioner type from the DbSet
            _dbContext.SubPrivillages.Remove(subPrivillages);

            // Save the changes to the database
            _dbContext.SaveChanges();
            TempData["isSuccessDelete"] = true;
            // Set the value in TempData
            TempData["isFromDeleteRequest"] = true;
            return RedirectToAction("subPrivillages");
        }


        [HttpGet]
        [Route("GetSubPrivillage/{id}")]
        public SubPrivillage GetSubPrivillage(int id)
        {
            var subPrivillages = _dbContext.SubPrivillages.Single(w => w.Id == id);
            return subPrivillages;
        }

        [Route("GetSubPrivillagesList")]
        public IActionResult GetSubPrivillagesList()
        {
            var subPrivillages = _dbContext.SubPrivillages.Include(i => i.Privillage).ToList();
            return PartialView("SubPrivillages", SubPrivillages);
        }
        #endregion

        #region UserServices
        [Route("Users/ServiceReviewRequests")]
        public IActionResult ServiceReviewRequests()
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

            return View(_dbContext.UserServices.Where(a => a.Status == Enums.ServicesStatusEnum.Pending).Include(a => a.UserProfile).ToList());
        }

        [Route("GetUserServices")]
        public IActionResult UserServicesList()
        {
            var userServices = _dbContext.UserServices.Where(a => a.Status == Enums.ServicesStatusEnum.Pending).Include(a => a.UserProfile).ToList();
            return PartialView("UserServicesList", userServices);
        }

        [HttpPost]
        [Route("AddUserServices")]
        public IActionResult AddUserServices([FromBody] UserServices userServices)
        {
            if (userServices == null || string.IsNullOrEmpty(userServices.TitleEn) || string.IsNullOrEmpty(userServices.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.UserServices.Any(w => w.TitleEn == userServices.TitleEn && w.TitleAr == userServices.TitleAr);
            if (isDuplicate)
            {
                return BadRequest("The details for the Practitioner Type have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(userServices.TitleAr) || !string.IsNullOrEmpty(userServices.TitleEn))
                {
                    _dbContext.UserServices.Add(userServices);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpPost]
        [Route("UpdateUserServices")]
        public IActionResult UpdateUserServices([FromBody] UserServices userServices)
        {
            if (userServices == null || string.IsNullOrEmpty(userServices.TitleEn) || string.IsNullOrEmpty(userServices.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.UserServices.Any(w => w.TitleEn == userServices.TitleEn && w.TitleAr == userServices.TitleAr);
            if (isDuplicate)
            {
                return BadRequest("The details for the Practitioner Type have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(userServices.TitleAr) || !string.IsNullOrEmpty(userServices.TitleEn))
                {
                    _dbContext.UserServices.Update(userServices);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("UpdateUserServicesStatus/{id}/{isActive}")]
        public IActionResult UpdateUserServicesStatus(int id, bool isActive)
        {
            // Logic to update the status of the practitioner type with the given ID
            try
            {
                var practitionerType = _dbContext.UserServices.SingleOrDefault(p => p.Id == id);
                if (practitionerType == null)
                {
                    return NotFound();
                }

                //practitionerType.IsActive = isActive;
                _dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the status.");
            }
        }

        [HttpGet]
        [Route("DeleteUserServices/{id}")]
        public IActionResult DeleteUserServices(int id)
        {

            bool isLinked = _dbContext.ApplicationUsers.Any(w => w.PractitionerTypeId == id);
            if (isLinked)
            {
                return BadRequest("You cannot delete this item as it is linked to users in the system.");
            }

            else
            {
                // Retrieve the practitioner type from the database using the id
                var userServices = _dbContext.UserServices.Find(id);


                if (userServices == null)
                {
                    // Handle the case where the practitioner type doesn't exist
                    TempData["isSuccessDelete"] = false;
                }

                // Remove the practitioner type from the DbSet
                _dbContext.UserServices.Remove(userServices);

                // Save the changes to the database
                _dbContext.SaveChanges();
                TempData["isSuccessDelete"] = true;
            }
            // Set the value in TempData
            TempData["isFromDeleteRequest"] = true;



            return RedirectToAction("ServiceReviewRequests");
        }
        [HttpGet]
        [Route("ApproveUserServices/{id}")]
        public IActionResult ApproveUserServices(int id)
        {


            var userServices = _dbContext.UserServices.Find(id);
            if (userServices != null && userServices.Status == Enums.ServicesStatusEnum.Pending)
                userServices.Status = Enums.ServicesStatusEnum.Approved;
            _dbContext.SaveChanges();
            //    if (userServices == null)
            //    {
            //        // Handle the case where the practitioner type doesn't exist
            //        TempData["isSuccessDelete"] = false;
            //    }

            //    // Remove the practitioner type from the DbSet
            //    _dbContext.UserServices.Remove(userServices);

            //    // Save the changes to the database
            //    _dbContext.SaveChanges();
            //    TempData["isSuccessDelete"] = true;
            ////}
            //// Set the value in TempData
            //TempData["isFromDeleteRequest"] = true;



            return RedirectToAction("ServiceReviewRequests");
        }

        [HttpGet]
        [Route("ApproveDashboardUserServices/{id}")]
        public IActionResult ApproveDashboardUserServices(int id)
        {


            var userServices = _dbContext.UserServices.Find(id);
            if (userServices != null && userServices.Status == Enums.ServicesStatusEnum.Pending)
                userServices.Status = Enums.ServicesStatusEnum.Approved;
            _dbContext.SaveChanges();
            //    if (userServices == null)
            //    {
            //        // Handle the case where the practitioner type doesn't exist
            //       TempData["isSuccessDelete"] = false;
            //    }

            //    // Remove the practitioner type from the DbSet
            //    _dbContext.UserServices.Remove(userServices);

            //    // Save the changes to the database
            //    _dbContext.SaveChanges();
            //    TempData["isSuccessDelete"] = true;
            ////}
            //// Set the value in TempData
            //TempData["isFromDeleteRequest"] = true;



            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        [Route("RejectDashboardUserServices")]
        public IActionResult RejectDashboardUserServices(int id, string rejectionReason)
        {


            var userServices = _dbContext.UserServices.Find(id);
            if (userServices != null && userServices.Status == Enums.ServicesStatusEnum.Pending)
            {
                userServices.Status = Enums.ServicesStatusEnum.Rejected;
                userServices.RejectionReason = rejectionReason;
            }
            else
            {
                userServices.RejectionReason = string.Empty;
            }
            _dbContext.SaveChanges();


            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        [Route("RejectUserServices/{id}")]
        public IActionResult RejectUserServices(int id)
        {

            //bool isLinked = _dbContext.ApplicationUsers.Any(w => w.PractitionerTypeId == id);
            //if (isLinked)
            //{
            //    return BadRequest("You cannot delete this item as it is linked to users in the system.");
            //}

            //else
            //{
            // Retrieve the practitioner type from the database using the id
            var userServices = _dbContext.UserServices.Find(id);
            if (userServices != null && userServices.Status == Enums.ServicesStatusEnum.Pending)
                userServices.Status = Enums.ServicesStatusEnum.Rejected;
            _dbContext.SaveChanges();
            //    if (userServices == null)
            //    {
            //        // Handle the case where the practitioner type doesn't exist
            //        TempData["isSuccessDelete"] = false;
            //    }

            //    // Remove the practitioner type from the DbSet
            //    _dbContext.UserServices.Remove(userServices);

            //    // Save the changes to the database
            //    _dbContext.SaveChanges();
            //    TempData["isSuccessDelete"] = true;
            ////}
            //// Set the value in TempData
            //TempData["isFromDeleteRequest"] = true;



            return RedirectToAction("ServiceReviewRequests");
        }
        [HttpGet]
        [Route("GetUserServices/{id}")]
        public UserServices GetUserServices(int id)
        {
            var userServices = _dbContext.UserServices.Single(w => w.Id == id);
            return userServices;
        }
        #endregion

        #region Countries
        [Route("MasterList/Countries")]
        public IActionResult Countries()
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

            return View(_dbContext.Countries.ToList());
        }

        [Route("GetCountries")]
        public IActionResult CountriesList()
        {
            var countrys = _dbContext.Countries.ToList();
            return PartialView("CountriesList", countrys);
        }

        [HttpPost]
        [Route("AddCountry")]
        public IActionResult AddCountry([FromBody] Country country)
        {
            if (country == null || string.IsNullOrEmpty(country.TitleEn) || string.IsNullOrEmpty(country.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.Countries.Any(w => w.TitleEn == country.TitleEn && w.TitleAr == country.TitleAr);
            if (isDuplicate)
            {
                return BadRequest("The details for the Practitioner Type have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(country.TitleAr) || !string.IsNullOrEmpty(country.TitleEn))
                {
                    _dbContext.Countries.Add(country);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpPost]
        [Route("UpdateCountry")]
        public IActionResult UpdateCountry([FromBody] Country country)
        {
            if (country == null || string.IsNullOrEmpty(country.TitleEn) || string.IsNullOrEmpty(country.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.Countries.Any(w => w.TitleEn == country.TitleEn && w.TitleAr == country.TitleAr);
            if (isDuplicate)
            {
                return BadRequest("The details for the Practitioner Type have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(country.TitleAr) || !string.IsNullOrEmpty(country.TitleEn))
                {
                    _dbContext.Countries.Update(country);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("DeleteCountry/{id}")]
        public IActionResult DeleteCountry(int id)
        {

            bool isLinked = _dbContext.States.Any(w => w.CountryId == id);
            if (isLinked)
            {
                TempData["isSuccessDelete"] = false;
            }

            else
            {
                // Retrieve the Country from the database using the id
                var country = _dbContext.Countries.Find(id);


                if (country == null)
                {
                    // Handle the case where the Country type doesn't exist
                    TempData["isSuccessDelete"] = false;
                }

                // Remove the practitioner type from the DbSet
                _dbContext.Countries.Remove(country);

                // Save the changes to the database
                _dbContext.SaveChanges();
                TempData["isSuccessDelete"] = true;


            }
            // Set the value in TempData
            TempData["isFromDeleteRequest"] = true;
            return RedirectToAction("Countries");
        }


        [HttpGet]
        [Route("GetCountry/{id}")]
        public Country GetCountry(int id)
        {
            var countrys = _dbContext.Countries.Single(w => w.Id == id);
            return countrys;
        }
        #endregion

        #region States
        [Route("MasterList/States")]
        public IActionResult States()
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

            return View(_dbContext.States.Include(i => i.Country).ToList());
        }


        [Route("GetStates")]
        public IActionResult StatesList()
        {
            var states = _dbContext.States.Include(i => i.Country).ToList();
            return PartialView("StatesList", states);
        }

        [HttpPost]
        [Route("AddState")]
        public IActionResult AddState([FromBody] State state)
        {
            if (state == null || string.IsNullOrEmpty(state.TitleEn) || string.IsNullOrEmpty(state.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.States
                                         .Any(w => (w.TitleEn == state.TitleEn && w.TitleAr == state.TitleAr)
                                         && w.CountryId == state.CountryId);
            if (isDuplicate)
            {
                return BadRequest("The details for the Professional Rank have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(state.TitleAr) || !string.IsNullOrEmpty(state.TitleEn))
                {
                    _dbContext.States.Add(state);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpPost]
        [Route("UpdateState")]
        public IActionResult UpdateState([FromBody] State state)
        {
            if (state == null || string.IsNullOrEmpty(state.TitleEn) || string.IsNullOrEmpty(state.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.States
                                        .Any(w => (w.TitleEn == state.TitleEn && w.TitleAr == state.TitleAr)
                                        && w.CountryId == state.CountryId);
            if (isDuplicate)
            {
                return BadRequest("The details for the Professional Rank have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(state.TitleAr) || !string.IsNullOrEmpty(state.TitleEn))
                {
                    _dbContext.States.Update(state);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("UpdateStateStatus/{id}/{isActive}")]
        public IActionResult UpdateStateStatus(int id, bool isActive)
        {
            // Logic to update the status of the practitioner type with the given ID
            try
            {
                var state = _dbContext.States.SingleOrDefault(p => p.Id == id);
                if (state == null)
                {
                    return NotFound();
                }

                _dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the status.");
            }
        }

        [HttpGet]
        [Route("DeleteState/{id}")]
        public IActionResult DeleteState(int id)
        {
            // Retrieve the practitioner type from the database using the id
            var state = _dbContext.States.Find(id);


            if (state == null)
            {
                // Handle the case where the practitioner type doesn't exist
                TempData["isSuccessDelete"] = false;
            }

            // Remove the practitioner type from the DbSet
            _dbContext.States.Remove(state);

            // Save the changes to the database
            _dbContext.SaveChanges();
            TempData["isSuccessDelete"] = true;
            // Set the value in TempData
            TempData["isFromDeleteRequest"] = true;
            return RedirectToAction("States");
        }


        [HttpGet]
        [Route("GetState/{id}")]
        public State GetState(int id)
        {
            var state = _dbContext.States.Single(w => w.Id == id);
            return state;
        }
        #endregion

        #region Cities
        [Route("MasterList/Cities")]
        public IActionResult Cities()
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

            return View(_dbContext.Cities.Include(i => i.Country)
                                          .Include(i => i.State)
                                          .ToList());
        }


        [Route("GetCities")]
        public IActionResult CitiesList()
        {
            var cities = _dbContext.Cities.Include(i => i.Country)
                                          .Include(i => i.State)
                                          .ToList();

            return PartialView("CitiesList", cities);
        }

        [HttpPost]
        [Route("AddCity")]
        public IActionResult AddCity([FromBody] City city)
        {
            if (city == null || string.IsNullOrEmpty(city.TitleEn) || string.IsNullOrEmpty(city.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.Cities
                                         .Any(w => (w.TitleEn == city.TitleEn && w.TitleAr == city.TitleAr)
                                         && w.CountryId == city.CountryId);
            if (isDuplicate)
            {
                return BadRequest("The details for the City have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(city.TitleAr) || !string.IsNullOrEmpty(city.TitleEn))
                {
                    city.CountryId = GetCountryIdByStateId(city.StateId);
                    _dbContext.Cities.Add(city);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }


        [HttpPost]
        [Route("UpdateCity")]
        public IActionResult UpdateCity([FromBody] City city)
        {
            if (city == null || string.IsNullOrEmpty(city.TitleEn) || string.IsNullOrEmpty(city.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.Cities
                                        .Any(w => (w.TitleEn == city.TitleEn && w.TitleAr == city.TitleAr)
                                        && w.CountryId == city.CountryId);
            if (isDuplicate)
            {
                return BadRequest("The details for the City have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(city.TitleAr) || !string.IsNullOrEmpty(city.TitleEn))
                {
                    _dbContext.Cities.Update(city);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }


        [HttpGet]
        [Route("DeleteCity/{id}")]
        public IActionResult DeleteCity(int id)
        {
            // Retrieve the practitioner type from the database using the id
            var city = _dbContext.Cities.Find(id);

            if (city == null)
            {
                // Handle the case where the practitioner type doesn't exist
                TempData["isSuccessDelete"] = false;
            }

            // Remove the practitioner type from the DbSet
            _dbContext.Cities.Remove(city);

            // Save the changes to the database
            _dbContext.SaveChanges();
            TempData["isSuccessDelete"] = true;
            // Set the value in TempData
            TempData["isFromDeleteRequest"] = true;
            return RedirectToAction("Cities");
        }


        [HttpGet]
        [Route("GetCity/{id}")]
        public City GetCity(int id)
        {
            var city = _dbContext.Cities.Single(w => w.Id == id);
            return city;
        }
        #endregion

        #region Districts
        [Route("MasterList/Districts")]
        public IActionResult Districts()
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

            return View(_dbContext.Districts.Include(i => i.Country)
                                          .Include(i => i.State)
                                          .ToList());
        }


        [Route("GetDistricts")]
        public IActionResult DistrictsList()
        {
            var cities = _dbContext.Districts.Include(i => i.Country)
                                          .Include(i => i.State)
                                          .Include(i => i.City)
                                          .ToList();

            return PartialView("DistrictsList", cities);
        }

        [HttpPost]
        [Route("AddDistrict")]
        public IActionResult AddDistrict([FromBody] District district)
        {
            if (district == null || string.IsNullOrEmpty(district.TitleEn) || string.IsNullOrEmpty(district.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.Districts
                                         .Any(w => (w.TitleEn == district.TitleEn && w.TitleAr == district.TitleAr)
                                         && w.CityId == district.CityId);
            if (isDuplicate)
            {
                return BadRequest("The details for the District have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(district.TitleAr) || !string.IsNullOrEmpty(district.TitleEn))
                {
                    district.CountryId = GetCountryIdByCityId(district.CityId);
                    district.StateId = GetStateIdByCityId(district.CityId);
                    _dbContext.Districts.Add(district);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }


        [HttpPost]
        [Route("UpdateDistrict")]
        public IActionResult UpdateDistrict([FromBody] District district)
        {
            if (district == null || string.IsNullOrEmpty(district.TitleEn) || string.IsNullOrEmpty(district.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.Districts
                                        .Any(w => (w.TitleEn == district.TitleEn && w.TitleAr == district.TitleAr)
                                        && w.CityId == district.CityId);
            if (isDuplicate)
            {
                return BadRequest("The details for the District have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(district.TitleAr) || !string.IsNullOrEmpty(district.TitleEn))
                {
                    district.CountryId = GetCountryIdByCityId(district.CityId);
                    district.StateId = GetStateIdByCityId(district.CityId);
                    _dbContext.Districts.Update(district);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }


        [HttpGet]
        [Route("DeleteDistrict/{id}")]
        public IActionResult DeleteDistrict(int id)
        {
            // Retrieve the practitioner type from the database using the id
            var district = _dbContext.Districts.Find(id);

            if (district == null)
            {
                // Handle the case where the practitioner type doesn't exist
                TempData["isSuccessDelete"] = false;
            }

            // Remove the practitioner type from the DbSet
            _dbContext.Districts.Remove(district);

            // Save the changes to the database
            _dbContext.SaveChanges();
            TempData["isSuccessDelete"] = true;
            // Set the value in TempData
            TempData["isFromDeleteRequest"] = true;
            return RedirectToAction("Districts");
        }


        [HttpGet]
        [Route("GetDistrict/{id}")]
        public District GetDistrict(int id)
        {
            var district = _dbContext.Districts.Single(w => w.Id == id);
            return district;
        }
        #endregion

        #region Services
        [Route("MasterList/Services")]
        public IActionResult Services()
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

            return View(_dbContext.Services.Include(a=>a.Privillage).Include(a=>a.SubPrivillage).ToList());
        }


        [Route("GetServices")]
        public IActionResult ServicesList()
        {
            var services = _dbContext.Services.Include(a=>a.Privillage).Include(a=>a.SubPrivillage).ToList();
            return PartialView("ServicesList", services);
        }

        [HttpPost]
        [Route("AddService")]
        public IActionResult AddService([FromBody] Services service)
        {
            if (service == null || string.IsNullOrEmpty(service.TitleEn) || string.IsNullOrEmpty(service.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.Services
                                         .Any(w => (w.TitleEn == service.TitleEn && w.TitleAr == service.TitleAr)
                                        );
            if (isDuplicate)
            {
                return BadRequest("The details for the Professional Rank have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(service.TitleAr) || !string.IsNullOrEmpty(service.TitleEn))
                {
                    _dbContext.Services.Add(service);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpPost]
        [Route("UpdateService")]
        public IActionResult UpdateService([FromBody] Services service)
        {
            if (service == null || string.IsNullOrEmpty(service.TitleEn) || string.IsNullOrEmpty(service.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.Services
                                        .Any(w => (w.TitleEn == service.TitleEn && w.TitleAr == service.TitleAr && w.Id != service.Id)
                                      );
            if (isDuplicate)
            {
                return BadRequest("The details for the services have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(service.TitleAr) || !string.IsNullOrEmpty(service.TitleEn))
                {
                    _dbContext.Services.Update(service);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("UpdateServiceStatus/{id}/{isActive}")]
        public IActionResult UpdateServiceStatus(int id, bool isActive)
        {
            // Logic to update the status of the practitioner type with the given ID
            try
            {
                var service = _dbContext.Services.SingleOrDefault(p => p.Id == id);
                if (service == null)
                {
                    return NotFound();
                }

                service.IsActive = isActive;
                _dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the status.");
            }
        }

        [HttpGet]
        [Route("DeleteService/{id}")]
        public IActionResult DeleteService(int id)
        {

            //bool isLinked = _dbContext.Services.Any(w => w.Id == id);
            //if (isLinked)
            //{
            //    return BadRequest("You cannot delete this item as it is linked to users in the system.");
            //}

            //else
            //{
            // Retrieve the practitioner type from the database using the id
            var service = _dbContext.Services.Find(id);


            if (service == null)
            {
                // Handle the case where the practitioner type doesn't exist
                TempData["isSuccessDelete"] = false;
            }

            // Remove the practitioner type from the DbSet
            _dbContext.Services.Remove(service);

            // Save the changes to the database
            _dbContext.SaveChanges();
            TempData["isSuccessDelete"] = true;
            //}
            // Set the value in TempData
            TempData["isFromDeleteRequest"] = true;
            return RedirectToAction("Services");
        }


        [HttpGet]
        [Route("GetService/{id}")]
        public Services GetService(int id)
        {
            var services = _dbContext.Services.Single(w => w.Id == id);
            return services;
        }
        #endregion

        #region Degrees
        [Route("MasterList/Degrees")]
        public IActionResult Degrees()
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

            return View(_dbContext.Degrees.ToList());
        }

        [Route("GetDegrees")]
        public IActionResult DegreesList()
        {
            var degrees = _dbContext.Degrees.ToList();
            return PartialView("DegreesList", degrees);
        }

        [HttpPost]
        [Route("AddDegree")]
        public IActionResult AddDegree([FromBody] Degree degree)
        {
            if (degree == null || string.IsNullOrEmpty(degree.TitleEn) || string.IsNullOrEmpty(degree.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.Degrees.Any(w => w.TitleEn == degree.TitleEn && w.TitleAr == degree.TitleAr);
            if (isDuplicate)
            {
                return BadRequest("The details for the Degree have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(degree.TitleAr) || !string.IsNullOrEmpty(degree.TitleEn))
                {
                    _dbContext.Degrees.Add(degree);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpPost]
        [Route("UpdateDegree")]
        public IActionResult UpdateDegree([FromBody] Degree degree)
        {
            if (degree == null || string.IsNullOrEmpty(degree.TitleEn) || string.IsNullOrEmpty(degree.TitleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.Degrees.Any(w => w.TitleEn == degree.TitleEn && w.TitleAr == degree.TitleAr);
            if (isDuplicate)
            {
                return BadRequest("The details for the Degree have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(degree.TitleAr) || !string.IsNullOrEmpty(degree.TitleEn))
                {
                    _dbContext.Degrees.Update(degree);
                    _dbContext.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("UpdateDegreeStatus/{id}/{isActive}")]
        public IActionResult UpdateDegreeStatus(int id, bool isActive)
        {
            // Logic to update the status of the practitioner type with the given ID
            try
            {
                var degree = _dbContext.Degrees.SingleOrDefault(p => p.Id == id);
                if (degree == null)
                {
                    return NotFound();
                }

                degree.IsActive = isActive;
                _dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the status.");
            }
        }

        [HttpGet]
        [Route("DeleteDegree/{id}")]
        public IActionResult DeleteDegree(int id)
        {
            // Retrieve the practitioner type from the database using the id
            var degree = _dbContext.Degrees.Find(id);

            if (degree == null)
            {
                // Handle the case where the practitioner type doesn't exist
                TempData["isSuccessDelete"] = false;
            }

            // Remove the practitioner type from the DbSet
            _dbContext.Degrees.Remove(degree);

            // Save the changes to the database
            _dbContext.SaveChanges();
            TempData["isSuccessDelete"] = true;
            // Set the value in TempData
            TempData["isFromDeleteRequest"] = true;
            return RedirectToAction("Degrees");
        }


        [HttpGet]
        [Route("GetDegree/{id}")]
        public Degree GetDegree(int id)
        {
            var degrees = _dbContext.Degrees.Single(w => w.Id == id);
            return degrees;
        }
        #endregion

        #region InsuranceCompanies
        [Route("MasterList/InsuranceCompanies")]
        public IActionResult InsuranceCompanies()
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

            return View(_dbContext.InsuranceCompanies.ToList());
        }

        [Route("GetInsuranceCompanies")]
        public IActionResult InsuranceCompaniesList()
        {
            var InsuranceCompanies = _dbContext.InsuranceCompanies.ToList();
            return PartialView("InsuranceCompaniesList", InsuranceCompanies);
        }

        [HttpPost]
        [Route("AddInsuranceCompany")]
        public IActionResult AddInsuranceCompany()
        {
            var insuranceCompany = new InsuranceCompany();

            // Get the form data
            var titleEn = Request.Form["titleEn"].ToString();
            var titleAr = Request.Form["titleAr"].ToString();
            var image = Request.Form.Files["image"];

            if (string.IsNullOrEmpty(titleEn) || string.IsNullOrEmpty(titleAr) || image == null)
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.InsuranceCompanies.Any(w => w.TitleEn == titleEn && w.TitleAr == titleAr);
            if (isDuplicate)
            {
                return BadRequest("The details for the Insurance Company have already been added.");
            }

            else
            {
                insuranceCompany.TitleEn = titleEn;
                insuranceCompany.TitleAr = titleAr;

                // Generate a unique file name
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

                // Save the image file to the specified path
                string webRootPath = _webHostEnvironment.WebRootPath;
                string uploadDir = Path.Combine("images", "InsuranceLogo");
                string imagePath = Path.Combine(webRootPath, uploadDir, uniqueFileName);
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }

                // Assign the image path to the LogoImagePath property
                insuranceCompany.LogoImagePath = $"/images/InsuranceLogo/{uniqueFileName}";
                insuranceCompany.IsActive = true;

                _dbContext.InsuranceCompanies.Add(insuranceCompany);
                _dbContext.SaveChanges();
            }

            return Ok();
        }


        [HttpPost]
        [Route("UpdateInsuranceCompany")]
        public IActionResult UpdateInsuranceCompany()
        {


            // Get the form data
            var titleEn = Request.Form["titleEn"].ToString();
            var titleAr = Request.Form["titleAr"].ToString();
            var id = Convert.ToInt32(Request.Form["titleId"].ToString());
            var image = Request.Form.Files["image"];

            var insuranceCompany = _dbContext.InsuranceCompanies.Where(w => w.Id == id).Single();

            if (string.IsNullOrEmpty(titleEn) || string.IsNullOrEmpty(titleAr))
            {
                return BadRequest("Please fill all fields.");
            }

            bool isDuplicate = _dbContext.InsuranceCompanies.Any(w => w.TitleEn == titleEn && w.TitleAr == titleAr && image == null);
            if (isDuplicate)
            {
                return BadRequest("The details for the Insurance Company have already been added.");
            }

            else
            {
                insuranceCompany.TitleEn = titleEn;
                insuranceCompany.TitleAr = titleAr;

                if (image != null)
                {
                    // Generate a unique file name
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

                    // Save the image file to the specified path
                    string webRootPath = _webHostEnvironment.WebRootPath;
                    string uploadDir = Path.Combine("images", "InsuranceLogo");
                    string imagePath = Path.Combine(webRootPath, uploadDir, uniqueFileName);
                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }

                    // Assign the image path to the LogoImagePath property
                    insuranceCompany.LogoImagePath = $"/images/InsuranceLogo/{uniqueFileName}";
                }

                _dbContext.InsuranceCompanies.Update(insuranceCompany);
                _dbContext.SaveChanges();
            }

            return Ok();
        }

        [HttpGet]
        [Route("UpdateInsuranceCompanyStatus/{id}/{isActive}")]
        public IActionResult UpdateInsuranceCompanyStatus(int id, bool isActive)
        {
            // Logic to update the status of the practitioner type with the given ID
            try
            {
                var insuranceCompany = _dbContext.InsuranceCompanies.SingleOrDefault(p => p.Id == id);
                if (insuranceCompany == null)
                {
                    return NotFound();
                }

                insuranceCompany.IsActive = isActive;
                _dbContext.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the status.");
            }
        }

        [HttpGet]
        [Route("DeleteInsuranceCompany/{id}")]
        public IActionResult DeleteInsuranceCompany(int id)
        {

            // Retrieve the practitioner type from the database using the id
            var insuranceCompany = _dbContext.InsuranceCompanies.Find(id);


            if (insuranceCompany == null)
            {
                // Handle the case where the practitioner type doesn't exist
                TempData["isSuccessDelete"] = false;
            }

            // Remove the practitioner type from the DbSet
            _dbContext.InsuranceCompanies.Remove(insuranceCompany);

            // Save the changes to the database
            _dbContext.SaveChanges();
            TempData["isSuccessDelete"] = true;

            // Set the value in TempData
            TempData["isFromDeleteRequest"] = true;



            return RedirectToAction("InsuranceCompanies");
        }


        [HttpGet]
        [Route("GetInsuranceCompany/{id}")]
        public InsuranceCompany GetInsuranceCompany(int id)
        {
            var InsuranceCompanies = _dbContext.InsuranceCompanies.Single(w => w.Id == id);
            return InsuranceCompanies;
        }
        #endregion

        #region Helper
        private int? GetCountryIdByStateId(int? stateId)
        {
            return _dbContext.States.SingleOrDefault(x => x.Id == stateId)?.CountryId;
        }

        private int? GetCountryIdByCityId(int? cityId)
        {
            return _dbContext.Cities.SingleOrDefault(x => x.Id == cityId)?.CountryId;
        }

        private int? GetStateIdByCityId(int? cityId)
        {
            return _dbContext.Cities.SingleOrDefault(x => x.Id == cityId)?.StateId;
        }
        #endregion

        #region Recent Profile Reviews Orders 
        [Route("GetProfileReviewsOrders")]
        public IActionResult GetProfileReviewsOrders()
        {
            return Json(_dbContext.UserProfiles.Include(i => i.PractitionerType).ToList());
        }

        [HttpGet]
        [Route("GetUserCertifications/{id}")]
        public IActionResult GetUserCertifications(int id)
        {
            return Json(_dbContext.Certifications.Where(x => x.UserProfileId == id).Include(i => i.Degree).ToList());
        }

        [HttpGet]
        [Route("RejectUserProfile/{id}/{rejectionReason}")]
        public ActionResult UpdateUserStatus(int id, string rejectionReason)
        {
            var userProfile = _dbContext.UserProfiles.Single(w => w.Id == id);
            userProfile.ProfileStatus = ProfileStatus.Rejected;
            userProfile.RejectionReason = rejectionReason;
            _dbContext.UserProfiles.Update(userProfile);
            _dbContext.SaveChanges();
            return Json(ProfileStatus.UnderReview.ToString());
        }

        [HttpGet]
        [Route("ApproveUserProfile/{id}")]
        public ActionResult ApproveUserProfile(int id)
        {
            var userProfile = _dbContext.UserProfiles.Single(w => w.Id == id);
            userProfile.ProfileStatus = ProfileStatus.Active;
            _dbContext.UserProfiles.Update(userProfile);
            _dbContext.SaveChanges();
            return Json(ProfileStatus.UnderReview.ToString());
        }
        #endregion
    }
}
