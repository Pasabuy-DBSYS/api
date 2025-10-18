using Microsoft.EntityFrameworkCore;
using PasabuyAPI.Data;
using PasabuyAPI.Enums;
using PasabuyAPI.Exceptions;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;

namespace PasabuyAPI.Repositories.Implementations
{
    public class VerificationInfoRepository(PasabuyDbContext _context) : IVerificationInfoRepository
    {
        public async Task<VerificationInfo> CreateVerificationInfoAsync(VerificationInfo verificationInfo)
        {
            await _context.VerificationInfo.AddAsync(verificationInfo);
            await _context.SaveChangesAsync();
            return verificationInfo;
        }

        public async Task<VerificationInfo> UpdateVerificationInfoByUserIdAsync(VerificationInfoStatus verificationInfoStatus, long userId)
        {
            VerificationInfo? verification = await _context.VerificationInfo
                                                .FirstOrDefaultAsync(v => v.UserIdFK == userId);

            if (verification == null) return null;

            verification.VerificationInfoStatus = verificationInfoStatus;
            verification.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return verification;
        }

        public async Task<VerificationInfo> UpdateInsuranceAsync(string insurancePath, long userId)
        {
            VerificationInfo target = await _context.VerificationInfo.FirstOrDefaultAsync(v => v.UserIdFK == userId)
                                        ?? throw new NotFoundException($"Verification info with userId: {userId} not found");

            target.InsurancePath = insurancePath;
            target.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return target;

        }
    }
}