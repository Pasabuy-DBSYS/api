using PasabuyAPI.Models;
using PasabuyAPI.Enums;

namespace PasabuyAPI.DTOs.Responses
{
    public class OrderResponseDTO
    {
        public long OrderIdPK { get; set; }
        public long CustomerId { get; set; }
        public long CourierId { get; set; }
        public string Request { get; set; } = string.Empty;
        
        public Status Status { get; set; }
        public bool IsCourierReviewed {get;set;}
        public bool IsCustomerReviewed {get;set;}
        public Priority Priority { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
        public DeliveryDetailsResponseDTO DeliveryDetailsDTO { get; set; } = null!;
        public PaymentsResponseDTO PaymentsResponseDTO { get; set; } = null!;
        public ChatRoomResponseDTO ChatRoomResponseDTO { get; set; } = null!;
    }
}