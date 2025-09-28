using Mapster;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Services.Interfaces;
using PasabuyAPI.DTOs.Requests;

namespace PasabuyAPI.Services.Implementations
{
    public class UserService(IUserRespository userRepository) : IUserService
    {
        private readonly IUserRespository _userRepository = userRepository;

        public async Task<UserResponseDTO?> GetUserByIdAsync(long id)
        {
            Users? user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
                return null;

            UserResponseDTO userResponse = user.Adapt<UserResponseDTO>();
            return userResponse;
        }

        public async Task<List<UserResponseDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Adapt<List<UserResponseDTO>>();
        }

        public async Task<UserResponseDTO> CreateUserAsync(UserRequestDTO user)
        {
            bool emailExists = await _userRepository.ExistsByEmailAsync(user.Email);
            if (emailExists)
                throw new InvalidOperationException("Email already exists.");

            bool usernameExists = await _userRepository.ExistsByUsernameAsync(user.Username);
            if (usernameExists)
                throw new InvalidOperationException("Username already exists.");

            var addedUser = await _userRepository.AddUserAsync(user.Adapt<Users>());

            return addedUser.Adapt<UserResponseDTO>();
        }
    }
}