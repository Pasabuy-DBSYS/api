using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Models;

namespace PasabuyAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDTO?> GetUserByIdAsync(long id);
        Task<List<UserResponseDTO>> GetAllUsersAsync();
        Task<UserResponseDTO> CreateUserAsync(UserRequestDTO user);
    }
}