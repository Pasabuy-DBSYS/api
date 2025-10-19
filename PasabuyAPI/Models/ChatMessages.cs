using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PasabuyAPI.Enums;

namespace PasabuyAPI.Models
{
    public class ChatMessages
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MessageIdPK { get; set; }
        [ForeignKey("ChatRoom")]
        public long RoomIdFK { get; set; }
        public ChatRooms ChatRoom { get; set; } = null!;
        [ForeignKey("Sender")]
        public long SenderIdFK { get; set; }
        public Users Sender { get; set; } = null!;
        [ForeignKey("Receiver")]
        public long ReceiverIdFK { get; set; }
        public Users Receiver { get; set; } = null!;
        public string Message { get; set; } = string.Empty;
        public MessageTypes MessageType { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReadAt { get; set; }
    }
}