using Microsoft.AspNetCore.Mvc;
using PasabuyAPI.Models;
using PasabuyAPI.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using PasabuyAPI.Services.Interfaces;
using System.Threading.Tasks;
using Mapster;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Xml.Schema;
using PasabuyAPI.Enums;

namespace PasabuyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService userService, IAwsS3Service awsS3Service) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet]
        public async Task<ActionResult<List<UserResponseDTO>>> GetUsersAsync()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<UserResponseDTO>> GetProfileAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized("Invalid token — user ID not found.");

            if (!long.TryParse(userIdClaim, out var userId))
                return BadRequest("Invalid user ID format.");

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDTO>> GetUserAsync(long id)
        {
            UserResponseDTO? user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        [Authorize]
        [HttpPatch("change/name")]
        public async Task<ActionResult<UserResponseDTO>> UpdateNameAsync( [FromBody] ChangeNameRequestDTO changeNameRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
                return Unauthorized("Invalid token — user ID not found.");

            if (!long.TryParse(userIdClaim, out var userId))
                return BadRequest("Invalid user ID format.");

            if (string.IsNullOrWhiteSpace(changeNameRequestDto.FirstName) || string.IsNullOrWhiteSpace(changeNameRequestDto.LastName))
                return BadRequest("Name cannot be empty.");

            var response = await _userService.UpdateNameAsync(userId, changeNameRequestDto);

            return Ok(response);
        }

        [Authorize]
        [HttpPatch("change/email")]
        public async Task<ActionResult<UserResponseDTO>> UpdateUserEmailAsync([FromBody] ChangeEmailRequestDTO changeEmailRequestDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
                return Unauthorized("Invalid token - user ID not found");

            if (!long.TryParse(userIdClaim, out var userId))
                return BadRequest("Invalid user ID format");

            if (string.IsNullOrWhiteSpace(changeEmailRequestDTO.Email))
                return BadRequest("Email cannot be empty");

            var response = await _userService.UpdateUserEmail(userId, changeEmailRequestDTO.Email);

            return Ok(response);
        }

        [Authorize]
        [HttpPatch("change/phone")]
        public async Task<ActionResult<UserResponseDTO>> UpdatePhoneAsync(
            [FromBody] ChangePhoneNumberRequestDTO changePhoneRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized("Invalid token — user ID not found.");

            if (!long.TryParse(userIdClaim, out var userId))
                return BadRequest("Invalid user ID format.");

            var response = await _userService.UpdatePhoneNumber(userId, changePhoneRequest.Phone);
            return Ok(response);
        }

        [Authorize]
        [HttpPatch("change/password")]
        public async Task<ActionResult<UserResponseDTO>> UpdatePasswordAsync(
            [FromBody] ChangePasswordRequestDTO changePasswordRequestDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized("Invalid token — user ID not found.");

            if (!long.TryParse(userIdClaim, out var userId))
                return BadRequest("Invalid user ID format.");

            var response = await _userService.UpdatePassword(userId, changePasswordRequestDTO.Password, changePasswordRequestDTO.ConfirmPassword);

            return Ok(response);
        }

        [Authorize]
        [HttpPatch("change/role/{role}")]
        public async Task<ActionResult<string>> UpdateRoleAsync(Roles role)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized("Invalid token — user ID not found.");

            if (!long.TryParse(userIdClaim, out var userId))
                return BadRequest("Invalid user ID format.");

            var response = await _userService.UpdateRole(userId, role);

            return Ok(response);
        }

        [HttpGet("signed-url")]
        public IActionResult GetSignedUrl([FromQuery] string key)
        {
            if (string.IsNullOrEmpty(key))
                return BadRequest("Missing file key. Example: /api/cloudfronttest/signed-url?key=ids/front_test.png");

            try
            {
                var url = awsS3Service.GenerateSignedUrl(key, TimeSpan.FromMinutes(10));
                return Ok(new
                {
                    FileKey = key,
                    SignedUrl = url,
                    ExpiresInMinutes = 10
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error generating signed URL", Details = ex.Message });
            }
        }
    
    }
}