using Eihal.Data;
using Eihal.Data.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Security.Claims;

namespace Eihal.Controllers
{
    [Route("File")]
    [Authorize(Roles = "Administrator,ServiceProvider")]
    public class FileController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileController(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
        }



        [HttpGet]
        [Route("DownloadFileFromFolder")]
        public FileResult DownloadFileFromFolder(string fileName)
        {
            //Build the File Path.  
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "uploadfiles\\") + fileName;

            //Read the File data into Byte Array.  
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.  
            return File(bytes, "application/octet-stream", fileName);
        }

        [HttpPost]
        [Route("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // Specify the directory where you want to save the uploaded file
                string webRootPath = _webHostEnvironment.WebRootPath;
                string attachmentsPath = Path.Combine("users", "attachments");
                string uploadPath = Path.Combine(webRootPath, attachmentsPath, currentUserId);
                // Create the directory if it doesn't exist
                Directory.CreateDirectory(uploadPath);
                // Generate a unique file name
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string fileExtension = Path.GetExtension(file.FileName);
                string uniqueFileName = fileName + "_" + Path.GetRandomFileName() + fileExtension;

                // Combine the directory and unique file name
                string filePath = Path.Combine(uploadPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    // Copy the file to the specified path
                    await file.CopyToAsync(stream);
                }
                var attachment = new Attachment()
                {
                    UserProfileId = GetUserId(),
                    Name = file.FileName,
                    Extension = fileExtension,
                    Path = Path.GetFileNameWithoutExtension(file.FileName),
                };

                _dbContext.Attachments.Add(attachment);

                // File uploaded successfully
                return Ok("File uploaded successfully.");
            }
            catch (IOException ex)
            {
                // Handle the error
                return StatusCode(StatusCodes.Status500InternalServerError, $"File upload failed: {ex.Message}");
            }
        }

        private int GetUserId()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _dbContext.UserProfiles.Where(w => w.UserId == currentUserId).Select(x => x.Id).Single();
        }

    }
}
