namespace PasabuyAPI.DTOs.Requests
{
    public class EmailVerificationRequestDTO
    {
        public string Email { get; set; }
        public string VerificationCode { get; set; }
    }
}