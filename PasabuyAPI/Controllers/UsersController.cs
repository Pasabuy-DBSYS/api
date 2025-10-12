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

namespace PasabuyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet]
        public async Task<ActionResult<List<UserResponseDTO>>> GetUsersAsync()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDTO>> GetUserAsync(long id)
        {
            UserResponseDTO? user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDTO>> CreateUserAsync([FromBody] UserRequestDTO userData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                UserResponseDTO user = await _userService.CreateUserAsync(userData);
                Console.WriteLine($"Created UserIdPK: {user.UserIdPK}");
                return CreatedAtAction(
                    actionName: nameof(GetUserAsync),
                    routeValues: new { id = user.UserIdPK },
                    value: user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}