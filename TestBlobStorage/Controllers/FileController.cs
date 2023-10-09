using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TestBlobStorage.Services;

namespace TestBlobStorage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IStorageManager _storageManager;

        public FileController(IStorageManager storageManager)
        {
            _storageManager = storageManager;
        }

        [HttpGet("getUrl")]
        public IActionResult GetUrl(string fileName)
        {
            try
            {
                var result = _storageManager.GetSignedUrl(fileName);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
        }

        [HttpPost("uploadAsync")]
        public async Task<IActionResult> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not selected or empty.");

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var contentType = file.ContentType;
                    
                    var fileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                    var uploadResult = await _storageManager.UploadFileAsync(stream, fileName, contentType);

                    if (uploadResult)
                        return Ok("File uploaded successfully.");
                    else
                        return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to upload the file.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"An error occurred: {ex.Message}");
            }
        } 

        [HttpPost("upload")]
        public  IActionResult UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not selected or empty.");

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var contentType = file.ContentType;
                    
                    var fileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                    var uploadResult =  _storageManager.UploadFile(stream, fileName, contentType);

                    if (uploadResult)
                        return Ok("File uploaded successfully.");
                    else
                        return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to upload the file.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        
        [HttpDelete("deleteAsync")]
        public async Task<IActionResult> DeleteFileAsync(string fileName)
        {
            try
            {
                var deleteResult = await _storageManager.DeleteFileAsync(fileName);

                if (deleteResult)
                    return Ok("File deleted successfully.");
                else
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to delete the file.");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }       
        [HttpDelete("delete")]
        public IActionResult DeleteFile(string fileName)
        {
            try
            {
                var deleteResult =  _storageManager.DeleteFile(fileName);

                if (deleteResult)
                    return Ok("File deleted successfully.");
                else
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to delete the file.");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}
