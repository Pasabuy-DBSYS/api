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
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [Authorize(Policy = "AdminOnly")]
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

        [Authorize(Policy = "VerifiedOnly")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDTO>> GetUserAsync(long id)
        {
            UserResponseDTO? user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        [Authorize]
        [HttpPatch("change/name")]
        public async Task<ActionResult<UserResponseDTO>> UpdateNameAsync([FromBody] ChangeNameRequestDTO changeNameRequestDto)
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
        [HttpPatch("change/profile")]
        public async Task<ActionResult<UserResponseDTO>> UpdateProfileAsync(
            [FromForm] ChangeProfilePictureRequestDTO changeProfileRequestDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized("Invalid token — user ID not found.");

            if (!long.TryParse(userIdClaim, out var userId))
                return BadRequest("Invalid user ID format.");

            var response = await _userService.UpdateProfilePicture(userId, changeProfileRequestDTO.ProfilePicture);

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

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("verification/{verification}")]
        public async Task<ActionResult<List<UserResponseDTO>>> GetUsersByVerificationStatus(VerificationInfoStatus verification)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
                return Unauthorized("Invalid token — user ID not found.");

            var response = await _userService.GetUsersByVerificationStatus(verification);

            return Ok(response);
        }

        [HttpPost("admin")]
        public async Task<ActionResult<UserResponseDTO>> CreateAdminAsync([FromBody] CreateAdminRequestDTO createAdminRequestDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _userService.AddAdmin(createAdminRequestDTO);

            return Ok(response);
        }

        [Authorize(Policy = "CustomerOnly")]
        [HttpGet("statistics/customer")]
        public async Task<ActionResult<CustomerStatisticsResponseDTO>> GetCustomerStatisticsAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized("Invalid token — user ID not found.");

            if (!long.TryParse(userIdClaim, out var userId))
                return BadRequest("Invalid user ID format.");

            var statistics = await _userService.GetCustomerStatistics(userId);
            return Ok(statistics);
        }

        [Authorize(Policy = "CourierOnly")]
        [HttpGet("statistics/courier")]
        public async Task<ActionResult<CourierStatisticsResponseDTO>> GetCourierStatisticsAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized("Invalid token — user ID not found.");

            if (!long.TryParse(userIdClaim, out var userId))
                return BadRequest("Invalid user ID format.");

            var statistics = await _userService.GetCourierStatistics(userId);
            return Ok(statistics);
        }

        [HttpGet("check/email/{email}")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email cannot be empty.");

            bool exists = await _userService.ExistsByEmailAsync(email);
            return Ok(new { exists });
        }

        [HttpGet("check/username/{username}")]
        public async Task<ActionResult<bool>> CheckUsernameExistsAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest("Username cannot be empty.");

            bool exists = await _userService.ExistsByUsernameAsync(username);
            return Ok(new { exists });
        }

        [HttpGet("check/phone/{phone}")]
        public async Task<ActionResult<bool>> CheckPhoneExistsAsync(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return BadRequest("Phone number cannot be empty.");

            bool exists = await _userService.ExistsByPhoneNumberUsernameAsync(phone);
            return Ok(new { exists });
        }
    }
}