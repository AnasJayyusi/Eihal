using Eihal.Data;
using Eihal.Data.Entites;
using Eihal.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Eihal.Data.SharedEnum;

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


        public void PushNewNotifications(NotificationTypeEnum type, int fromUserId, int toUserId, string? additionalData)
        {
            var notification = new Notification();
            notification.CreatedByUserId = fromUserId;
            notification.AssignedToUserId = toUserId;
            notification.CreationDate = DateTime.Now;

            switch (type)
            {
                case NotificationTypeEnum.NewOrder:
                    notification.TitleAr = "طلب جديد";
                    notification.TitleEn = "New Order";
                    notification.MessageEn = $"New Have New Order With ID {additionalData}";
                    notification.MessageAr = $"لديك طلب جديد رقم الطلب {additionalData}";
                    break;
                case NotificationTypeEnum.ApprovedOrder:
                    notification.TitleAr = "قبول طلب";
                    notification.TitleEn = "Approved Order";
                    notification.MessageEn = $"Order With ID {additionalData} Approved";
                    notification.MessageAr = $"طلب رقم الطلب {additionalData} مقبول";
                    break;
                case NotificationTypeEnum.RejectOrder:
                    notification.TitleAr = "رفض طلب";
                    notification.TitleEn = "Reject Order";
                    notification.MessageEn = $"Order With ID {additionalData} Rejected";
                    notification.MessageAr = $"طلب رقم الطلب {additionalData} مرفوض";
                    break;
            }

            _dbContext.Notifications.Add(notification);
            _dbContext.SaveChanges();

        }

    }
}

