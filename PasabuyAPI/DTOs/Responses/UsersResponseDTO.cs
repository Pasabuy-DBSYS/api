using PasabuyAPI.Enums;
using PasabuyAPI.Models;


namespace PasabuyAPI.DTOs.Responses
{
    public class UserResponseDTO
    {
        public long UserIdPK { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateOnly Birthday { get; set; }
        public decimal RatingAverage { get; set; } = 0m;
        public long TotalDeliveries { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Roles CurrentRole { get; set; }
        public string ProfilePictureKey { get; set; } = string.Empty;
        public VerificationInfoResponseDTO VerifiactionInfoDTO { get; set; } = null!;
    }
}
