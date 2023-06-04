using Eihal.Data;
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

        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }


        [Route("MasterList/PractitionerTypes")]
        public IActionResult PractitionerTypes()
        {
            return View(_dbContext.PractitionerTypes.ToList());
        }


        [Route("MasterList/ProfessionalRank")]
        public IActionResult ProfessionalRank()
        {
            return View();
        }

    }
}
