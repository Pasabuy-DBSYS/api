using Microsoft.AspNetCore.Mvc;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task <ActionResult<string>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            return await authenticationService.Login(loginRequestDTO);
        }
    }
}