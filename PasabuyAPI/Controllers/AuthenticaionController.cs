using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController(IAuthenticationService authenticationService, IAwsS3Service awsS3Service, IUserService userService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var result = await authenticationService.Login(loginRequestDTO);
                return Ok(new { message = "Login Successful", token = result });
            }
            catch (Exception e)
            {
                return StatusCode(400, e);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDTO>> CreateUserAsync([FromForm] UserRequestDTO userData)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Save files to AWS Bucket
                var paths = await awsS3Service.UploadIDs(userData.FrontId, userData.BackId, userData.Insurance);

                userData.FrontIdPath = paths.FrontIdFileName;
                userData.BackIdPath = paths.BackIdFileName;
                userData.InsurancePath = paths.InsuranceFileName;

                // Proceed to create the user
                var user = await userService.CreateUserAsync(userData);

                return Ok(new { message = "Register Successful" , data = user.Adapt<UserResponseDTO>()});
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("verifystatus")]
        public async Task<IActionResult> VerifyUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
                return Forbid("Invalid token");

            if (!long.TryParse(userIdClaim, out var userId))
                return Forbid("Invalid Id");

            string token = await userService.VerifyUser(userId);

            return Ok(new { token = token });
        }
    }
}