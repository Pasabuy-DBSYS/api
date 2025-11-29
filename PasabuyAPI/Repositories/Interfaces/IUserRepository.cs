using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Enums;
using PasabuyAPI.Models;

namespace PasabuyAPI.Repositories.Interfaces
{
    public interface IUserRespository
    {
        Task<Users?> GetUserByIdAsync(long id);
        Task<List<Users>> GetAllUsersAsync();
        Task<Users> AddUserAsync(Users user);

        Task<Users> UpdateNameAsync(long userId, string firstName, string middleNamem, string lastName);
        Task<Users> UpdateUserEmail(long userId, string email);
        Task<Users> UpdatePhoneNumber(long userId, string phoneNumber);
        Task<Users> UpdatePassword(long userId, string password);
        Task<Users> UpdateProfilePicture(long userId, string pfpPath);
        Task<string> UpdateRole(long userId, Roles role);

        Task<Users> AddAdmin(Users user);
        Task<List<Users>> GetUsersByVerificationStatus(VerificationInfoStatus verificationInfoStatus);


        // helpers
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<bool> ExistsByPhoneAsync(string phone);
        Task<bool> VerifyUser(long userId);
        Task<CustomerStatisticsResponseDTO> GetCustomerStatistics(long userId);
        Task<CourierStatisticsResponseDTO> GetCourierStatistics(long userId);
    }
}