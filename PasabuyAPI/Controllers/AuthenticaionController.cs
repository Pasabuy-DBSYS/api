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
        public async Task<ActionResult<string>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            return await authenticationService.Login(loginRequestDTO);
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

                return user.Adapt<UserResponseDTO>();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}