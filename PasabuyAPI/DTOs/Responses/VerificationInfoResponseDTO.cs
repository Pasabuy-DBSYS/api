using PasabuyAPI.Enums;

namespace PasabuyAPI.DTOs.Responses
{
    public class VerificationInfoResponseDTO
    {
        public long VerifiactionInfoId { get; set; }
        public long UserIdFK { get; set; }
        public string FrontIdPath { get; set; } = string.Empty;
        public string BackIdPath { get; set; } = string.Empty;
        public string InsurancePath { get; set; } = string.Empty;
        public VerificationInfoStatus VerificationInfoStatus { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}