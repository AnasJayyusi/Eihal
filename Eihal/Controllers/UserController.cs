using Microsoft.AspNetCore.Mvc;

namespace Eihal.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
