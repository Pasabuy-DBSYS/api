namespace PasabuyAPI.DTOs.Responses
{
    public class EmailVerificationResponseDTO
    {
        public long EmailVerificationId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string VerificationCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsVerified { get; set; }
    }
}