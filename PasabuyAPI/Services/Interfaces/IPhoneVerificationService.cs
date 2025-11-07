using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Enums;

namespace PasabuyAPI.Services.Interfaces
{
    public interface IPhoneVerificationServices
    {
        Task<PhoneVerificationResponseDTO> CreateOrUpdateVerificationCode(string phone);
        Task<VerificationResult> VerifyVerificationCode(string phone, string code);
    }
}