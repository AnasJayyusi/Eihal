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

        public string GetUserImage()
        {
            var userId = GetUserProfileId();
            var userProfilePath = _dbContext.UserProfiles.Where(w => w.Id == userId).Select(a => a.ProfilePicturePath).FirstOrDefault();
            if (userProfilePath == null)
            {
                return "/users/images/Default-User-Profile.jpg";
            }
            return userProfilePath;
        }

        public void SetImagePathInCookies()
        {
            var cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(1);
            cookieOptions.Path = "/";
            Response.Cookies.Append("userImagePath", GetUserImage(), cookieOptions);
        }
    }
}
