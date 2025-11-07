using Mapster;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Enums;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Services.Implementations
{
    public class PhoneVerificationServices(IPhoneVerificationRepository phoneVerificationRepository) : IPhoneVerificationServices
    {
        public async Task<PhoneVerificationResponseDTO> CreateOrUpdateVerificationCode(string phone)
        {
            var result = await phoneVerificationRepository.CreateOrUpdateVerificationAsync(phone);
            
            return result.Adapt<PhoneVerificationResponseDTO>();
        }

        public async Task<VerificationResult> VerifyVerificationCode(string phone, string code)
        {
            var result = await phoneVerificationRepository.VerifyPhoneNumber(phone, code);
            return result;
        }
    }
}