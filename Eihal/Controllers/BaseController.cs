using Eihal.Data;
using Microsoft.AspNetCore.Mvc;

namespace Eihal.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ApplicationDbContext _dbContext;

        public BaseController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
