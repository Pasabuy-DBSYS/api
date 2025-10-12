using Microsoft.EntityFrameworkCore;
using PasabuyAPI.Data;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;

namespace PasabuyAPI.Repositories.Implementations
{
    public class UserRepository(PasabuyDbContext context) : IUserRespository
    {

        public async Task<Users?> GetUserByIdAsync(long id)
        {
            return await context.Users
                    .Include(u => u.VerifiactionInfo)
                    .FirstOrDefaultAsync(u => u.UserIdPK == id);
        }

        public async Task<List<Users>> GetAllUsersAsync()
        {
            return await context.Users
                .Include(u => u.VerifiactionInfo)
                .ToListAsync();
        }

        public async Task<Users> AddUserAsync(Users user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
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


    }
}