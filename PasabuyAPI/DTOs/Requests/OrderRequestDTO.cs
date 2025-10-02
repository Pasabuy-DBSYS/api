using PasabuyAPI.Enums;

namespace PasabuyAPI.DTOs.Requests
{
    public class OrderRequestDTO
    {
        public long CustomerId { get; set; }
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; }
        public Priority Priority { get; set; }
    }
}