using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Service;
using Entity;
using DataRepository;


namespace webProjeact.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class FileUploadController : Controller
    {
        private readonly IFileUploadBL _fileUploadBL;

        public FileUploadController(IFileUploadBL fileUploadBL)
        {
            _fileUploadBL = fileUploadBL;
        }

        [HttpPost("upload")]
        public ActionResult<string[]> UploadFile(IFormFile[] files)
        {
            try
            {
                if (files == null || files.Length == 0)
                {
                    return Ok();
                }

                // Iterate through each file in the array
                List<string> fileUrls = new List<string>();
                foreach (var file in files)
                {
                    if (file == null || file.Length == 0)
                    {
                        continue; // Skip empty files
                    }

                    // Upload logic for individual file
                    string fileUrl = _fileUploadBL.UploadFiles(file);
                    fileUrls.Add(fileUrl); // Add URL to the list
                }

                // Return the list of file URLs
                return Ok(fileUrls);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPost("ProcessEmployeeExcelFile")]
        public ActionResult<bool> ProcessEmployeeExcelFile(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);
                        memoryStream.Position = 0;

                        // Call the corresponding method in the BL layer
                        return _fileUploadBL.ProcessEmployeeExcelFile(memoryStream);
                    }
                }
                else
                {
                    return BadRequest("No file or empty file provided");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
