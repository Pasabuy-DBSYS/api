using System.Net;
using System.Net.Mail;
using Microsoft.VisualBasic;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Services.Implementations
{
    public class EmailServices(IConfiguration _config) : IEmailServices
    {
        private readonly string email = _config["EmailSettings:SenderEmail"];
        private readonly string password = _config["EmailSettings:AppPassword"];
        private readonly int port = int.TryParse(_config["EmailSettings:Port"], out var p) ? p : 587;
        private readonly string smtp = _config["EmailSettings:SmtpServer"];


        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient(smtp, port)
            {
                Credentials = new NetworkCredential(email, password),
                EnableSsl = true,
            };

            using var message = new MailMessage(email, to)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true // if needed
            };

            await client.SendMailAsync(message);
            return true;
        }
    }
}