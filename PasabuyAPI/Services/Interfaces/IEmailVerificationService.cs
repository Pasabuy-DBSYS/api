using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Enums;
using PasabuyAPI.Models;

namespace PasabuyAPI.Services.Interfaces
{
    public interface IEmailVerificationService
    {
        Task<EmailVerificationResponseDTO> CreateOrUpdateVerificationAsync(string email);
        Task<VerificationResult> VerifyEmail(string email, string verification);
    }
}