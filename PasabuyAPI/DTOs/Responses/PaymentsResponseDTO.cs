using PasabuyAPI.Enums;
using PasabuyAPI.Models;

namespace PasabuyAPI.DTOs.Responses
{
    public class PaymentsResponseDTO
    {
        public long PaymentIdPK { get; set; }
        public long OrderIdFK { get; set; }
        public Guid TransactionId { get; set; }
        public decimal BaseFee { get; set; }
        public decimal UrgencyFee { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal? TipAmount { get; set; }
        public decimal? ItemsFee { get; set; }
        public decimal? ProposedItemsFee { get; set; }

        public decimal? TotalAmount { get; set; }
        public bool IsItemsFeeConfirmed { get; set; }
        public string ImageKey { get; set; } = string.Empty;
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime PaidAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}