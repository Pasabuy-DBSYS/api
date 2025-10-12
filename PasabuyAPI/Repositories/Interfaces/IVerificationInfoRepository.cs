using PasabuyAPI.Enums;
using PasabuyAPI.Models;

namespace PasabuyAPI.Repositories.Interfaces
{
    public interface IVerificationInfoRepository
    {
        Task<VerificationInfo> CreateVerificationInfoAsync(VerificationInfo verificationInfo);
        Task<VerificationInfo> UpdateVerificationInfoByUserIdAsync(VerificationInfoStatus verificationInfoStatus, long userId);
    }
}