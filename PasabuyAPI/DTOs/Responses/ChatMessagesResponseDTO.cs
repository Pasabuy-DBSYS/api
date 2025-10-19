using PasabuyAPI.Enums;
using PasabuyAPI.Models;

namespace PasabuyAPI.DTOs.Responses
{
    public class ChatMessagesResponseDTO
    {
        public long MessageIdPK { get; set; }
        public long RoomIdFK { get; set; }
        public long SenderIdFK { get; set; }
        public long ReceiverIdFK { get; set; }
        public string Message { get; set; } = string.Empty;
        public MessageTypes MessageType { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReadAt { get; set; }
    }
}