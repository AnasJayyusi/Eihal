using Eihal.Data.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Eihal.Controllers
{

    [Route("ServiceProvider")]
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        [Route("MyProfile")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Referrals()
        {
            return View();
        }


        [HttpPost]
        public JsonResult Index(string kw)
        {
            if (string.IsNullOrEmpty(kw))
            {
                kw = "";
            }
            var compaines = new List<InsuranceCompany>()
            {

                new InsuranceCompany {Id=1,NameEn="InsuranceCompany1",NameAr="شركة التامين الاولى",Base64Image="data:image/png;base64,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" },
                new InsuranceCompany {Id=2,NameEn="InsuranceCompany2",NameAr="شركة التامين الثاني" },
        };
            var Name = (from c in compaines
                        where c.NameAr.Contains(kw) || c.NameEn.Contains(kw)
                        select new { c.Id, c.NameAr, c.NameEn, c.Base64Image });
            return Json(Name);
        }
    }
}

