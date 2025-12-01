using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PasabuyAPI.Enums;

namespace PasabuyAPI.Models
{
    public class Payments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PaymentIdPK { get; set; }
        public long OrderIdFK { get; set; }
        public Orders Order { get; set; } = null!;
        public Guid TransactionId { get; set; } = Guid.NewGuid();

        [Column(TypeName = "decimal(10,2)")]
        public decimal BaseFee { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? UrgencyFee { get; set; } = 0;

        [Column(TypeName = "decimal(10,2)")]
        public decimal DeliveryFee { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? TipAmount { get; set; } = 0;

        [Column(TypeName = "decimal(10,2)")]
        public decimal? ItemsFee { get; set; } = 0;
        public decimal? ProposedItemsFee { get; set; } = 0;
        public string ImageKey { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal? TotalAmount { get; set; } = 0;
        public bool IsItemsFeeConfirmed { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}