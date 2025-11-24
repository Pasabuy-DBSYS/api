using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasabuyAPI.Models
{
    public class DeliveryDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DeliveryIdPk { get; set; }

        public long OrderIdFK { get; set; }
        public Orders Order { get; set; } = null!;

        [Column(TypeName = "decimal(9,6)")]
        public decimal LocationLatitude { get; set; }
       
        [Column(TypeName = "decimal(9,6)")]
        public decimal LocationLongitude { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal CourierLatitude { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal CourierLongitude { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal CustomerLatitude { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal CustomerLongitude { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal ActualDistance { get; set; }
        public string DestinationAddress { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
        public DateTime EstimatedDeliveryTime { get; set; }
        public DateTime? ActualPickupTime { get; set; }
        public DateTime? ActualDeliveryTime { get; set; }
        public string DeliveryNotes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}