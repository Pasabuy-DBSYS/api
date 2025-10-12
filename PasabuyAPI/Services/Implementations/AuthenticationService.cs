using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Services.Implementations
{
    public class AuthenticationService(IAuthenticationRepository authenticationRepository) : IAuthenticationService
    {
        public Task<string> Login(LoginRequestDTO loginRequest)
        {
            return authenticationRepository.Login(loginRequest.Username, loginRequest.Password);
        }
    }
}