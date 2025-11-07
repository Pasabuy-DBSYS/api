using PasabuyAPI.Enums;
using PasabuyAPI.Models;

namespace PasabuyAPI.Repositories.Interfaces
{
    public interface IPhoneVerificationRepository
    {
        Task<PhoneVerification> CreateOrUpdateVerificationAsync(string phoneNumber);
        Task<VerificationResult> VerifyPhoneNumber(string phoneNumber, string verification);
    }
}