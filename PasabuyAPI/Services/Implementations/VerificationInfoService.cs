using Mapster;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Enums;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Services.Implementations
{
    public class VerificationInfoService(IVerificationInfoRepository verificationInfoRepository) : IVerificationInfoService
    {
        public async Task<VerificationInfoResponseDTO> CreateVerificationInfo(VerificationInfoRequestDTO requestDTO)
        {
            VerificationInfo verificationInfo = await verificationInfoRepository.CreateVerificationInfoAsync(requestDTO.Adapt<VerificationInfo>());
            return verificationInfo.Adapt<VerificationInfoResponseDTO>();
        }

        public async Task<VerificationInfoResponseDTO> UpdateVerificationInfoByUserIdAsync(VerificationInfoStatus verificationInfoStatus, long userId)
        {
            VerificationInfo verificationInfo = await verificationInfoRepository.UpdateVerificationInfoByUserIdAsync(verificationInfoStatus, userId);

            return verificationInfo.Adapt<VerificationInfoResponseDTO>();
        }
    }
}