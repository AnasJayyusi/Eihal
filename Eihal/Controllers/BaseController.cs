using Eihal.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eihal.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ApplicationDbContext _dbContext;
        public string _loggedUserId;

        public BaseController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int GetUserId()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _dbContext.UserProfiles.Where(w => w.UserId == currentUserId).Select(x => x.Id).Single();
        }


    }
}
