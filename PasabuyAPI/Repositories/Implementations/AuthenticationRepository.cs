using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PasabuyAPI.Configurations.Jwt;
using PasabuyAPI.Data;
using PasabuyAPI.Enums;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;

namespace PasabuyAPI.Repositories.Implementations
{
    public class AuthenticationRepository(PasabuyDbContext context, TokenProvider tokenProvider) : IAuthenticationRepository
    {
        public async Task<string> Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new Exception("Username cannot be empty");

            // Look for user by username or email
            var user = await context.Users
                .Include(u => u.VerificationInfo)
                .FirstOrDefaultAsync(u => u.Username == username || u.Email == username)
                ?? throw new Exception("User not found");

            var passwordHasher = new PasswordHasher<Users>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Password is incorrect");

            if (user.VerificationInfo.VerificationInfoStatus == VerificationInfoStatus.REJECTED)
                throw new Exception($"User verification status is '{user.VerificationInfo.VerificationInfoStatus}'. Access denied.");

            // Generate JWT token
            var token = tokenProvider.Create(user);
            return token;
        }
    }
}