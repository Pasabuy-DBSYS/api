namespace PasabuyAPI.DTOs.Responses
{
    public class PhoneVerificationResponseDTO
    {
        public long PhoneVerificationId { get; set; }
        public string PhoneNumber { get; set; }
        public string VerificationCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsVerified { get; set; }
    }
}