

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourcesController(IAwsS3Service awsS3Service) : ControllerBase
    {
        [Authorize]
        [HttpGet("signed-url")]
        public IActionResult GetSignedUrl([FromQuery] string key)
        {
            if (string.IsNullOrEmpty(key))
                return BadRequest("Missing file key");

            try
            {
                var url = awsS3Service.GenerateSignedUrl(key, TimeSpan.FromMinutes(60));
                return Ok(new
                {
                    FileKey = key,
                    SignedUrl = url,
                    ExpiresInMinutes = 60
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error generating signed URL", Details = ex.Message });
            }
        }
    }
}