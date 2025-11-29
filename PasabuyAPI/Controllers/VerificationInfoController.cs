using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = "AdminOnly")]
        [HttpPatch("{userId}")]
        public async Task<ActionResult<VerificationInfoResponseDTO>> UpdateVerificationInfoByUserIdAsync([FromBody] VerificationInfoStatus verificationInfoStatus, long userId)
        {
            VerificationInfoResponseDTO verificationInfoResponseDTO = await verificationInfoService.UpdateVerificationInfoByUserIdAsync(verificationInfoStatus, userId);

            if (verificationInfoResponseDTO is null) return NotFound($"User Id {userId} not found");

            return Ok(verificationInfoResponseDTO);
        }

        [Authorize]
        [HttpPatch("insurance")]
        public async Task<ActionResult<VerificationInfoResponseDTO>> UpdateInsuranceByUserIdAsync([FromForm] UpdateInsuranceRequestDTO updateInsuranceRequestDTO)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized("Invalid token — user ID not found.");

            if (!long.TryParse(userIdClaim, out var userId))
                return BadRequest("Invalid user ID format.");

            VerificationInfoResponseDTO verificationInfoResponseDTO = await verificationInfoService.UpdateInsuranceAsync(updateInsuranceRequestDTO.Insurance, userId);

            if (verificationInfoResponseDTO is null) return NotFound($"User Id {userId} not found");

            return Ok(verificationInfoResponseDTO);
        }
    }
}