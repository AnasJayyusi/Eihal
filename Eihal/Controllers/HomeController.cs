using Eihal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Eihal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            if (User.IsInRole("User"))
            {
                return RedirectToAction("Index", "User");
            }
            else if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else
                return View();

        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }


        [Authorize(Roles = "Administrator")]
        public IActionResult AdminOnly()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}