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
        public long CustomerId{ get; set; }
        public Users Customer { get; set; } = null!;
        public long CourierId { get; set; }
        public Users Courier { get; set; } = null!;

        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public DateTime Created_at { get; set; } = DateTime.UtcNow;
        public DateTime Updated_at { get; set; } = DateTime.UtcNow;
    }
}