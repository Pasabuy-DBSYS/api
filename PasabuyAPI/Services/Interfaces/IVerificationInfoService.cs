using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Enums;

namespace PasabuyAPI.Services.Interfaces
{
    public interface IVerificationInfoService
    {
        Task<VerificationInfoResponseDTO> CreateVerificationInfo(VerificationInfoRequestDTO requestDTO);
        Task<VerificationInfoResponseDTO> UpdateVerificationInfoByUserIdAsync(VerificationInfoStatus verificationInfoStatus, long userId);
        Task<VerificationInfoResponseDTO> UpdateInsuranceAsync(IFormFile insurance, long userId);

    }
}