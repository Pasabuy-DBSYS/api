using Microsoft.EntityFrameworkCore;
using PasabuyAPI.Data;
using PasabuyAPI.Enums;
using PasabuyAPI.Exceptions;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;

namespace PasabuyAPI.Repositories.Implementations
{
    public class EmailVerificationRepository(PasabuyDbContext context) : IEmailVerificationRepository
    {
        public async Task<EmailVerification> CreateOrUpdateVerificationAsync(string email)
        {
            string code = new Random().Next(10000, 99999).ToString();
            EmailVerification emailVerification = await context.EmailVerifications.FirstOrDefaultAsync(e => e.Email == email);

            if (emailVerification != null)
            {
                emailVerification.VerificationCode = code;
                emailVerification.UpdatedAt = DateTime.UtcNow;
                emailVerification.ExpiresAt = DateTime.UtcNow.AddMinutes(5);
                await context.SaveChangesAsync();
                return emailVerification;
            }

            var newEmail = new EmailVerification
            {
                Email = email,
                VerificationCode = code,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                UpdatedAt = DateTime.UtcNow,
                IsVerified = false
            };

            await context.EmailVerifications.AddAsync(newEmail);
            await context.SaveChangesAsync();
            return newEmail;
            
        }

        public async Task<VerificationResult> VerifyEmail(string email, string verification)
        {
            EmailVerification emailVerification = await context.EmailVerifications.FirstOrDefaultAsync(e => e.Email == email) ??
                                        throw new NotFoundException("Phone number not found");

            if (emailVerification.ExpiresAt < DateTime.UtcNow)
                return VerificationResult.Expired;

            if (emailVerification.VerificationCode != verification.Trim())
                return VerificationResult.Invalid;

            emailVerification.IsVerified = true;
            emailVerification.ExpiresAt = DateTime.UtcNow; // expire immediately

            await context.SaveChangesAsync();

            return VerificationResult.Success;
        }
    }
}