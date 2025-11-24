using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PasabuyAPI.Configurations.Jwt;
using PasabuyAPI.Data;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.Enums;
using PasabuyAPI.Exceptions;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;

namespace PasabuyAPI.Repositories.Implementations
{
    public class UserRepository(PasabuyDbContext context, IPasswordHasher<Users> passwordHasher, TokenProvider tokenProvider) : IUserRespository
    {

        public async Task<Users?> GetUserByIdAsync(long id)
        {
            return await context.Users
                    .Include(u => u.VerificationInfo)
                    .FirstOrDefaultAsync(u => u.UserIdPK == id);
        }

        public async Task<List<Users>> GetAllUsersAsync()
        {
            return await context.Users
                .Include(u => u.VerificationInfo)
                .ToListAsync();
        }

        public async Task<Users> AddUserAsync(Users user)
        {
            user.PasswordHash = passwordHasher.HashPassword(user, user.PasswordHash);

            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }
        public async Task<Users> UpdateNameAsync(long userId, string firstName, string middleName, string lastName)
        {
            Users target = await context.Users.FirstOrDefaultAsync(u => u.UserIdPK == userId)
                            ?? throw new NotFoundException($"User with id: {userId} not found");

            target.FirstName = firstName;
            target.MiddleName = middleName;
            target.LastName = lastName;

            await context.SaveChangesAsync();
            return target;
        }

        public async Task<Users> UpdateUserEmail(long userId, string email)
        {
            Users target = await context.Users.FirstOrDefaultAsync(u => u.UserIdPK == userId)
                            ?? throw new NotFoundException($"User with id: {userId} not found");

            var exists = await ExistsByEmailAsync(email);

            if (exists) throw new AlreadyExistsException($"Email: {email} already exists");
            
            try
            {
                var mail = new MailAddress(email); // throws if invalid
                target.Email = mail.Address.Trim(); // Normalize the email
                await context.SaveChangesAsync();
                return target;
            }
            catch (FormatException)
            {
                throw new InvalidEmailFormatException($"Invalid email format for email: {email}");
            }
        }


        public async Task<Users> UpdatePhoneNumber(long userId, string phoneNumber)
        {
            Users target = await context.Users.FirstOrDefaultAsync(u => u.UserIdPK == userId)
                            ?? throw new NotFoundException($"User with id: {userId} not found");

            // Optional: Check if phone already exists
            var exists = await ExistsByPhoneAsync(phoneNumber);
            if (exists)
                throw new AlreadyExistsException($"Phone number: {phoneNumber} already exists");

            // Validate format — simple pattern, adjust for your locale if needed
            var phonePattern = @"^\+?[1-9]\d{7,14}$"; // E.164 international format
            if (!Regex.IsMatch(phoneNumber, phonePattern))
                throw new InvalidPhoneNumberFormatException($"Invalid phone number format: {phoneNumber}");

            target.Phone = phoneNumber.Trim();
            await context.SaveChangesAsync();

            return target;
        }


        public async Task<Users> UpdatePassword(long userId, string password)
        {
            Users target = await context.Users.FirstOrDefaultAsync(u => u.UserIdPK == userId)
                            ?? throw new NotFoundException($"User with id: {userId} not found");

            // var strongPassword = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#]).{8,}$");
            // if (!strongPassword.IsMatch(password))
            //     throw new WeakPasswordException("Password must be at least 8 characters and include uppercase, lowercase, number, and special character.");

            target.PasswordHash = passwordHasher.HashPassword(target, password);
            await context.SaveChangesAsync();
            return target;
        }

        public async Task<string> UpdateRole(long userId, Roles role)
        {
            Users target = await context.Users.Include(u => u.VerificationInfo).FirstOrDefaultAsync(u => u.UserIdPK == userId)
                            ?? throw new NotFoundException($"User with id: {userId} not found");

            if (await HasActiveOrderAsync(userId)) throw new CannotUpdateRoleException("You have an active order. You cannot change roles now");

            target.CurrentRole = role;
            await context.SaveChangesAsync();
            string token = tokenProvider.Create(target);
            return token;
        }

        public async Task<Users> UpdateProfilePicture(long userId, string pfpPath)
        {
            Users target = await context.Users.Include(u => u.VerificationInfo).FirstOrDefaultAsync(u => u.UserIdPK == userId)
                            ?? throw new NotFoundException($"User with id: {userId} not found");

            target.ProfilePictureKey = pfpPath;
            await context.SaveChangesAsync();

            return target;
        }

        // helpers
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await context.Users.AnyAsync(u => u.Email == email);
        }
        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> ExistsByPhoneAsync(string phone)
        {
            return await context.Users.AnyAsync(u => u.Phone == phone);
        }

        public async Task<bool> HasActiveOrderAsync(long userId)
        {
            Status[] activeStatuses = [Status.PENDING, Status.ACCEPTED, Status.IN_TRANSIT];

            return await context.Orders.AnyAsync(o =>
                (o.CustomerId == userId || o.CourierId == userId) &&
                activeStatuses.Contains(o.Status));
        }

        public async Task<bool> VerifyUser(long userId)
        {
            return await context.Users.AnyAsync(u => u.UserIdPK == userId &&
                    u.VerificationInfo.VerificationInfoStatus >= VerificationInfoStatus.ACCEPTED);
        }

        public async Task<Users> AddAdmin(Users user)
        {
            user.PasswordHash = passwordHasher.HashPassword(user, user.PasswordHash);
            user.CurrentRole = Roles.ADMIN;

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<List<Users>> GetUsersByVerificationStatus(VerificationInfoStatus verificationInfoStatus)
        {
            return await context.Users
                            .Include(u => u.VerificationInfo.VerificationInfoStatus == verificationInfoStatus)
                            .ToListAsync();
        }
    }
}
