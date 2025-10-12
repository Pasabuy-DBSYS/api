using PasabuyAPI.Models;

namespace PasabuyAPI.Repositories.Interfaces
{
    public interface IAuthenticationRepository
    {
        Task<string> Login(string username, string password);
    }
}