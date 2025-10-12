
using PasabuyAPI.DTOs.Requests;

namespace PasabuyAPI.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> Login(LoginRequestDTO loginRequest);
    }
}