namespace PasabuyAPI.DTOs.Responses
{
    public class PhoneVerificationResponseDTO
    {
        public long PhoneVerificationId { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string VerificationCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsVerified { get; set; }
    }
}