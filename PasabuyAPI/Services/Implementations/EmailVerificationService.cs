using Mapster;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Enums;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Services.Implementations
{
    public class EmailVerificationService(IEmailVerificationRepository emailVerificationRepository, IEmailServices emailServices, IWebHostEnvironment env) : IEmailVerificationService
    {
        public async Task<EmailVerificationResponseDTO> CreateOrUpdateVerificationAsync(string email)
        {
            var result = await emailVerificationRepository.CreateOrUpdateVerificationAsync(email);

            var templatePath = Path.Combine(env.ContentRootPath, "EmailTemplates", "VerificationCode.html");
            var htmlBody = await File.ReadAllTextAsync(templatePath);


            htmlBody = htmlBody
                .Replace("{{email}}", email)
                .Replace("{{appName}}", "PasaBuy")
                .Replace("{{code}}", result.VerificationCode)
                .Replace("{{expiryMinutes}}", "5")
                .Replace("{{companyName}}", "PasaBuy")
                .Replace("{{companyAddress}}", "Cebu, Philippines");

            emailServices.SendEmailAsync(email, "PasaBuy Verification Code", htmlBody);

            return result.Adapt<EmailVerificationResponseDTO>();
        }

        public async Task<VerificationResult> VerifyEmail(string email, string verification)
        {
            var result = await emailVerificationRepository.VerifyEmail(email, verification);
            return result;
        }
    }
}