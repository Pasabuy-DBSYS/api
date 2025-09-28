using PasabuyAPI.Models;

namespace PasabuyAPI.Repositories.Interfaces
{
    public interface IUserRespository
    {
        Task<Users?> GetUserByIdAsync(long id);
        Task<List<Users>> GetAllUsersAsync();
        Task<Users> AddUserAsync(Users user);

        // helpers
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<bool> ExistsByPhoneAsync(string phone);
    }
}