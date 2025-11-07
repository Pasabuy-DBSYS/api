using PasabuyAPI.Enums;
using PasabuyAPI.Models;

namespace PasabuyAPI.Repositories.Interfaces
{
    public interface IEmailVerificationRepository
    {
        Task<EmailVerification> CreateOrUpdateVerificationAsync(string email);
        Task<VerificationResult> VerifyEmail(string email, string verification);
    }
}