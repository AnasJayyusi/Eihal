using Eihal.Data;
using Eihal.Data.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                return BadRequest("The details for the Professional Rank have already been added.");
            }

            else
            {
                if (!string.IsNullOrEmpty(city.TitleAr) || !string.IsNullOrEmpty(city.TitleEn))
                {
                    city.CountryId = GetCountryId(city.StateId);
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
                return BadRequest("The details for the Professional Rank have already been added.");
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

            return View(_dbContext.Services.ToList());
        }


        [Route("GetServices")]
        public IActionResult ServicesList()
        {
            var services = _dbContext.Services.ToList();
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
                                        .Any(w => (w.TitleEn == service.TitleEn && w.TitleAr == service.TitleAr)
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


        #region Helper
        private int? GetCountryId(int? stateId)
        {
            return _dbContext.States.SingleOrDefault(x => x.Id == stateId)?.CountryId;
        }
        #endregion
    }
}
