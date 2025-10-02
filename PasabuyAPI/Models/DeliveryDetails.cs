using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasabuyAPI.Models
{
    public class DeliveryDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DeliveryIdPk { get; set; }

        public long? OrderIdFK { get; set; }
        public Orders Order { get; set; } = null!;
        public decimal EstimatedDistance { get; set; }
        public decimal ActualDistance { get; set; }
        public decimal CourierLatitude { get; set; }
        public decimal CourierLongitude { get; set; }
        public decimal CustomerLatitude { get; set; }
        public decimal CustomerLongitude { get; set; }
        public DateTime EstimatedDeliveryTime { get; set; }
        public DateTime? ActualDeliveryTime { get; set; }
        public decimal DeliveryFee { get; set; }
        public string DeliveryNotes { get; set; } = string.Empty;
    }
}