using Eihal.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
namespace Eihal.Controllers
{

    [Route("Notification")]
    [Authorize(Roles = "Administrator,ServiceProvider,Beneficiary")]
    public class NotificationController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;

        public NotificationController(UserManager<IdentityUser> userManager,ApplicationDbContext dbContext) : base(dbContext)
        {
            _userManager = userManager;
        }


        [HttpGet]
        [Route("GetUserNotificationsCount")]
        public ActionResult GetUserNotificationsCount()
        {
            var currentUserId = GetUserProfileId();
            //var user =  _userManager.GetUserAsync(User);
            //if (user != null)
            //{
            //    // Get the user ID
            //    var userId =  _userManager.GetUserIdAsync(user.Result);

            //    // Now, you have the current user ID (userId)
            //    // You can use it as needed
            //}
            var count = _dbContext.Notifications.Where(a => a.AssignedToUserId == currentUserId && a.IsRead == false).Count();

            // Pass the data to the view
            return Json(count);
        }

        [HttpGet]
        [Route("GetUserNotifications")]
        public ActionResult GetUserNotifications()
        {
            var currentUserId = GetUserProfileId();
            //var user =  _userManager.GetUserAsync(User);
            //if (user != null)
            //{
            //    // Get the user ID
            //    var userId =  _userManager.GetUserIdAsync(user.Result);

            //    // Now, you have the current user ID (userId)
            //    // You can use it as needed
            //}
            var model = _dbContext.Notifications.Where(a => a.AssignedToUserId == currentUserId).OrderByDescending(a => a.CreationDate).ToList();
            // Pass the data to the view
            return PartialView("_NotificationsList", model);

        }

        [HttpGet]
        [Route("MarkAllNotificationsReaded")]
        public ActionResult MarkAllNotificationsReaded()
        {
            var currentUserId = GetUserProfileId();
 
            var model = _dbContext.Notifications.Where(a => a.AssignedToUserId == currentUserId).OrderByDescending(a => a.CreationDate).ToList();
            foreach (var item in model)
            {
                item.IsRead = true;
            }
            _dbContext.SaveChanges();
            return Ok();
        }

    }
}
