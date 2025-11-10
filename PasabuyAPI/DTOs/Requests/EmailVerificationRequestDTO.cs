namespace PasabuyAPI.DTOs.Requests
{
    public class EmailVerificationRequestDTO
    {
        public required string Email { get; set; }
        public required string VerificationCode { get; set; }
    }
}