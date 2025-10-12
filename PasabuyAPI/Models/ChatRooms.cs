using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace PasabuyAPI.Models
{
    public class ChatRooms
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RoomIdPK { get; set; }

        [ForeignKey("Order")]        
        public long OrderIdFK { get; set; }
        public Orders Order { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ClosedAt { get; set; }
        public bool IsActive { get; set; }
        public List<ChatMessages> ChatMessages { get; set; } = [];
    }
}