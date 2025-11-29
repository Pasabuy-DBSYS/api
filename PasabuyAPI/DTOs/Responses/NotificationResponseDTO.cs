namespace PasabuyAPI.DTOs.Responses
{
    public class NotificationResponseDTO
    {
        public long NotificationPkId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } =string.Empty;
        public bool Pressed { get; set; }
        public long UserIdFK { get; set; } 
    }
}