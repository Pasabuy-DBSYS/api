using Microsoft.EntityFrameworkCore;
using PasabuyAPI.Data;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;

namespace PasabuyAPI.Repositories.Implementations
{
    public class UserRepository(PasabuyDbContext context) : IUserRespository
    {
        private readonly PasabuyDbContext _context = context;

        public async Task<Users?> GetUserByIdAsync(long id)
        {
            return await _context.Users
                // .Include(u => u.CustomerOrders)
                //     .ThenInclude(o => o.DeliveryDetails)
                // .Include(u => u.CourierOrders)
                //     .ThenInclude(o => o.DeliveryDetails)
                .FirstOrDefaultAsync(u => u.UserIdPK == id);
        }

        public async Task<List<Users>> GetAllUsersAsync()
        {
            return await _context.Users
                // .Include(u => u.CustomerOrders)
                //     .ThenInclude(o => o.DeliveryDetails)
                // .Include(u => u.CourierOrders)
                //     .ThenInclude(o => o.DeliveryDetails)
                .ToListAsync();
        }

        public async Task<Users> AddUserAsync(Users user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // helpers
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> ExistsByPhoneAsync(string phone)
        {
            return await _context.Users.AnyAsync(u => u.Phone == phone);
        }


    }
}