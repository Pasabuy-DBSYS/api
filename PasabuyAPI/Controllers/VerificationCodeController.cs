using System.Net;
using Microsoft.AspNetCore.Mvc;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Enums;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerificationCodeController(IPhoneVerificationServices phoneVerificationServices, IEmailVerificationService emailVerificationService) : ControllerBase
    {
        [HttpGet("phone/{phoneNumber}")]
        public async Task<ActionResult<PhoneVerificationResponseDTO>> GetPhoneVerificationCode(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return BadRequest("Phone number cannot be empty");

            PhoneVerificationResponseDTO response = await phoneVerificationServices.CreateOrUpdateVerificationCode(phoneNumber);

            return StatusCode(201, response);
        }
        
        [HttpPost("phone/verify")]
        public async Task<ActionResult<PhoneVerificationResponseDTO>> VerifyPhoneVerificationCode([FromBody] VerifyPhoneRequestDTO verifyPhoneRequestDTO)
        {
            if (string.IsNullOrEmpty(verifyPhoneRequestDTO.PhoneNumber) || string.IsNullOrEmpty(verifyPhoneRequestDTO.Code))
                return BadRequest("Phone number and code cannot be empty");

            var result = await phoneVerificationServices.VerifyVerificationCode(verifyPhoneRequestDTO.PhoneNumber, verifyPhoneRequestDTO.Code);

            if (result != VerificationResult.Success)
                return BadRequest($"Invalid code for phone: {verifyPhoneRequestDTO.PhoneNumber}");

            return StatusCode(200, result.ToString());
        }

        [HttpGet("email/{emailAddress}")]
        public async Task<ActionResult<EmailVerificationResponseDTO>> GetEmailVerificationCode(string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
                return BadRequest("Email cannot be empty");

            EmailVerificationResponseDTO response = await emailVerificationService.CreateOrUpdateVerificationAsync(emailAddress);

            return StatusCode(201, response);
        } 

        [HttpPost("email/verify")]
        public async Task<ActionResult<PhoneVerificationResponseDTO>> VerifyEmailVerificationCode([FromBody] EmailVerificationRequestDTO verifyEmail)
        {
            if (string.IsNullOrEmpty(verifyEmail.Email) || string.IsNullOrEmpty(verifyEmail.VerificationCode))
                return BadRequest("Email and code cannot be empty");

            var result = await emailVerificationService.VerifyEmail(verifyEmail.Email, verifyEmail.VerificationCode);

            if (result != VerificationResult.Success)
                return BadRequest($"Invalid code for email: {verifyEmail.Email}");
                
            return StatusCode(200, result.ToString());
        }                   
    }
}