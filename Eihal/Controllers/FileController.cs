using Eihal.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Eihal.Controllers
{
    public class FileController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _hostenvironment;

        public FileController(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = applicationDbContext;
            _hostenvironment = webHostEnvironment;
        }

        [HttpGet]
        public FileResult DownloadFileFromFolder(string fileName)
        {
            //Build the File Path.  
            string path = Path.Combine(_hostenvironment.WebRootPath, "uploadfiles\\") + fileName;

            //Read the File data into Byte Array.  
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.  
            return File(bytes, "application/octet-stream", fileName);
        }
    }
}
