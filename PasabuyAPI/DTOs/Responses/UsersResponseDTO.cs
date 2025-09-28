using PasabuyAPI.Enums;
using PasabuyAPI.Models;


namespace PasabuyAPI.DTOs.Responses
{
    public class UserResponseDTO
    {
        public long UserIdPK { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateOnly Birthday { get; set; }
        public YearLevel YearLevel { get; set; }
        public decimal RatingAverage { get; set; } = 0m;
        public long TotalDeliveries { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        // Use DTOs inside DTOs — do NOT reference EF entity types here (avoid Users/Orders model classes).
        public List<OrderResponseDTO> CustomerOrders { get; set; } = [];
        public List<OrderResponseDTO> CourierOrders { get; set; } = [];
    }
}
