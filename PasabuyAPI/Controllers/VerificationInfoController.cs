using Microsoft.AspNetCore.Mvc;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Enums;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerificationInfoController(IVerificationInfoService verificationInfoService) : ControllerBase
    {
        [HttpPatch("{userId}")]
        public async Task<ActionResult<VerificationInfoResponseDTO>> UpdateVerificationInfoByUserIdAsync([FromBody] VerificationInfoStatus verificationInfoStatus, long userId)
        {
            VerificationInfoResponseDTO verificationInfoResponseDTO = await verificationInfoService.UpdateVerificationInfoByUserIdAsync(verificationInfoStatus, userId);

            if (verificationInfoResponseDTO is null) return NotFound($"User Id {userId} not found");

            return Ok(verificationInfoResponseDTO);
        }
    }
}