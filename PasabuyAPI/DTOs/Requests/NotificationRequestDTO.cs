namespace PasabuyAPI.DTOs.Requests
{
    public class NotificationRequestDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool Pressed{ get; set; } = false;
        public long UserIdFk { get; set; }
    }
}