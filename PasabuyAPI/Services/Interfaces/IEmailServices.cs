namespace PasabuyAPI.Services.Interfaces
{
    public interface IEmailServices
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);
    }
}