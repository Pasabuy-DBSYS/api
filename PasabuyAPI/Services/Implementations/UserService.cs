using Mapster;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Services.Interfaces;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.Enums;

namespace PasabuyAPI.Services.Implementations
{
    public class UserService(IUserRespository userRepository, IVerificationInfoRepository verificationInfoRepository) : IUserService
    {
        public async Task<UserResponseDTO?> GetUserByIdAsync(long id)
        {
            Users? user = await userRepository.GetUserByIdAsync(id);

            if (user == null)
                return null;

            UserResponseDTO userResponse = user.Adapt<UserResponseDTO>();
            return userResponse;
        }

        public async Task<List<UserResponseDTO>> GetAllUsersAsync()
        {
            var users = await userRepository.GetAllUsersAsync();
            return users.Adapt<List<UserResponseDTO>>();
        }

        public async Task<UserResponseDTO> CreateUserAsync(UserRequestDTO user)
        {
            bool emailExists = await userRepository.ExistsByEmailAsync(user.Email);
            if (emailExists)
                throw new InvalidOperationException("Email already exists.");

            bool usernameExists = await userRepository.ExistsByUsernameAsync(user.Username);
            if (usernameExists)
                throw new InvalidOperationException("Username already exists.");

            var addedUser = await userRepository.AddUserAsync(user.Adapt<Users>());

            VerificationInfo verification = new()
            {
                UserIdFK = addedUser.UserIdPK,
                FrontIdPath = user.FrontIdPath,
                BackIdPath = user.BackIdPath,
                VerificationInfoStatus = VerificationInfoStatus.PENDING,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var addedVerification = await verificationInfoRepository.CreateVerificationInfoAsync(verification);

            var response = addedUser.Adapt<UserResponseDTO>();
            response.VerifiactionInfoDTO = addedVerification.Adapt<VerificationInfoResponseDTO>();
            return response;
        }
    }
}