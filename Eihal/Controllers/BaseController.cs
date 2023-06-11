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


    }
}
