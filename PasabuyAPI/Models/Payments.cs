using PasabuyAPI.Enums;

namespace PasabuyAPI.Models
{
    public class Payments
    {
        public long PaymentId { get; set; }
        public long OrderIdFK { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal? TipAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public Guid TransactionId { get; set; }
        public DateTime PaidAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}