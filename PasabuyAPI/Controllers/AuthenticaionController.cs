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
    public class AuthenticationController(IAuthenticationService authenticationService, IUserService userService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var token = await authenticationService.Login(loginRequestDTO);
                return Ok(new { message = "Login Successful", token });
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

            return Ok(new { token });
        }
    }
}