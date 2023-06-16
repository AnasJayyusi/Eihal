using Eihal.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eihal.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ApplicationDbContext _dbContext;
        public string _loggedAspNetUserId;
        public int? _userProfileId;

        public BaseController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int GetUserProfileId()
        {
            string currentUserProfileId = GetAspNetUserId();
            if (_userProfileId == null)
                return _dbContext.UserProfiles.Where(w => w.UserId == currentUserProfileId).Select(x => x.Id).Single();
            else
                return _userProfileId.Value;
        }

        public string GetAspNetUserId()
        {
            if (_loggedAspNetUserId == null)
            {
                _loggedAspNetUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return _loggedAspNetUserId;
            }
            else
                return _loggedAspNetUserId;

        }


    }
}
