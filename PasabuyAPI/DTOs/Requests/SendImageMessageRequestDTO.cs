using PasabuyAPI.Enums;

namespace PasabuyAPI.DTOs.Requests
{
    public class SendImageMessageRequestDTO
    {
        public long RoomIdFK { get; set; }
        public long SenderIdFK { get; set; }
        public long ReceiverIdFK { get; set; }
        public required IFormFile Image { get; set; }
        public string Message { get; set; } = string.Empty;
        public MessageTypes MessageType { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}