using Microsoft.AspNetCore.Mvc;

namespace Eihal.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
