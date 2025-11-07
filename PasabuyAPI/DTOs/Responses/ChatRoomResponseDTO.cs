namespace PasabuyAPI.DTOs.Responses
{
    public class ChatRoomResponseDTO
    {
        public long RoomIdPK { get; set; }
        public long OrderIdFK { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public bool IsActive { get; set; }
    }
}