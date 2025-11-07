using Mapster;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Enums;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Services.Implementations
{
    public class EmailVerificationService(IEmailVerificationRepository emailVerificationRepository) : IEmailVerificationService
    {
        public async Task<EmailVerificationResponseDTO> CreateOrUpdateVerificationAsync(string email)
        {
            var result = await emailVerificationRepository.CreateOrUpdateVerificationAsync(email);
            return result.Adapt<EmailVerificationResponseDTO>();

        }

        public async Task<VerificationResult> VerifyEmail(string email, string verification)
        {
            var result = await emailVerificationRepository.VerifyEmail(email, verification);
            return result;
        }
    }
}