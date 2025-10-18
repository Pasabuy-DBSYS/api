using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Enums;
using PasabuyAPI.Models;

namespace PasabuyAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDTO?> GetUserByIdAsync(long id);
        Task<List<UserResponseDTO>> GetAllUsersAsync();
        Task<UserResponseDTO> CreateUserAsync(UserRequestDTO user);
        Task<UserResponseDTO> UpdateNameAsync(long userId, ChangeNameRequestDTO changeNameRequestDto);
        Task<UserResponseDTO> UpdateUserEmail(long userId, string email);
        Task<UserResponseDTO> UpdatePhoneNumber(long userId, string phoneNumber);
        Task<UserResponseDTO> UpdatePassword(long userId, string password, string confirmation);
        Task<UserResponseDTO> UpdateRole(long userId, Roles role);
    }
}