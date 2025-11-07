using Mapster;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Services.Interfaces;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.Enums;
using PasabuyAPI.Exceptions;
using PasabuyAPI.Configurations.Jwt;

namespace PasabuyAPI.Services.Implementations
{
    public class UserService(IUserRespository userRepository, IVerificationInfoRepository verificationInfoRepository, TokenProvider tokenProvider) : IUserService
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
                InsurancePath = user.InsurancePath,
                VerificationInfoStatus = VerificationInfoStatus.PENDING,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var addedVerification = await verificationInfoRepository.CreateVerificationInfoAsync(verification);

            var response = addedUser.Adapt<UserResponseDTO>();
            response.VerifiactionInfoDTO = addedVerification.Adapt<VerificationInfoResponseDTO>();
            return response;
        }

        public async Task<UserResponseDTO> UpdateNameAsync(long userId, ChangeNameRequestDTO changeNameRequestDto)
        {
            var updatedUser = await userRepository.UpdateNameAsync(userId, changeNameRequestDto.FirstName, changeNameRequestDto.MiddleName ?? string.Empty, changeNameRequestDto.LastName);
            
            return updatedUser.Adapt<UserResponseDTO>();
        }

        public async Task<UserResponseDTO> UpdateUserEmail(long userId, string email)
        {
            var updatedUser = await userRepository.UpdateUserEmail(userId, email);
            return updatedUser.Adapt<UserResponseDTO>();
        }

        public async Task<UserResponseDTO> UpdatePhoneNumber(long userId, string phoneNumber)
        {
            var updatedUser = await userRepository.UpdatePhoneNumber(userId, phoneNumber);
            return updatedUser.Adapt<UserResponseDTO>();        
        }

        public async Task<UserResponseDTO> UpdatePassword(long userId, string password, string confirmation)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmation))
                throw new EmptyFieldException("Password cannot be empty");
            
            if (password != confirmation)
                throw new PasswordMismatchException("Password mismatch");


            var updatedUser = await userRepository.UpdatePassword(userId, password);
            return updatedUser.Adapt<UserResponseDTO>();
        }

        public async Task<string> UpdateRole(long userId, Roles role)
        {
            return await userRepository.UpdateRole(userId, role);
        }

        public async Task<string> VerifyUser(long userId)
        {
            if (!await userRepository.VerifyUser(userId))
                return null;

            Users user = await userRepository.GetUserByIdAsync(userId) ?? throw new NotFoundException("User not found");

            return tokenProvider.Create(user);

        }
    }
}