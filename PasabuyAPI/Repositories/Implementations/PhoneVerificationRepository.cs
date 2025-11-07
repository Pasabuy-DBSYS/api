using Microsoft.EntityFrameworkCore;
using PasabuyAPI.Data;
using PasabuyAPI.Enums;
using PasabuyAPI.Exceptions;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;

namespace PasabuyAPI.Repositories.Implementations
{
    public class PhoneVerificationRepository(PasabuyDbContext context) : IPhoneVerificationRepository
    {
        public async Task<PhoneVerification> CreateOrUpdateVerificationAsync(string phoneNumber)
        {
            var code = new Random().Next(10000, 99999).ToString();
            PhoneVerification phone = await context.PhoneVerifications.FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber);

            if (phone != null)
            {
                phone.VerificationCode = code;
                phone.UpdatedAt = DateTime.UtcNow;
                phone.ExpiresAt = DateTime.UtcNow.AddMinutes(5);
                await context.SaveChangesAsync();
                return phone;
            }

            // Create new record
            var newPhone = new PhoneVerification
            {
                PhoneNumber = phoneNumber,
                VerificationCode = code,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            };

            await context.PhoneVerifications.AddAsync(newPhone);
            await context.SaveChangesAsync();

            return newPhone;
        }

        public async Task<VerificationResult> VerifyPhoneNumber(string phoneNumber, string verification)
        {
            PhoneVerification phone = await context.PhoneVerifications.FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber) ??
                                        throw new NotFoundException("Phone number not found");

            if (phone.ExpiresAt < DateTime.UtcNow)
                return VerificationResult.Expired;

            if (phone.VerificationCode != verification.Trim())
                return VerificationResult.Invalid;

            phone.IsVerified = true;
            phone.ExpiresAt = DateTime.UtcNow; // expire immediately

            await context.SaveChangesAsync();

            return VerificationResult.Success;
        }
    }
}