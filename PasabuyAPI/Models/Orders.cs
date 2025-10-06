using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using PasabuyAPI.Enums;

namespace PasabuyAPI.Models
{
    public class Orders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderIdPK { get; set; }
        public long CustomerId { get; set; }
        public Users Customer { get; set; } = null!;
        public long? CourierId { get; set; }
        public Users? Courier { get; set; }

        public string Request { get; set; } = string.Empty;
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }
        [EnumDataType(typeof(Priority))]
        public Priority Priority { get; set; }
        public DateTime Created_at { get; set; } = DateTime.UtcNow;
        public DateTime Updated_at { get; set; } = DateTime.UtcNow;

        public DeliveryDetails DeliveryDetails { get; set; } = null!;
        public Payments Payment { get; set; } = null!;
    }
}