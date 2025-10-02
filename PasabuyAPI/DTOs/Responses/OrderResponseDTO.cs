using PasabuyAPI.Models;
using PasabuyAPI.Enums;

namespace PasabuyAPI.DTOs.Responses
{
    public class OrderResponseDTO
    {
        public long OrderIdPK { get; set; }
        public long CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public long CourierId { get; set; }
        public string CourierName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; } 
        public DeliveryDetailsResponseDTO? DeliveryDetailsDTO { get; set; }
    }
}