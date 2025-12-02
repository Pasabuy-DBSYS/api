using PasabuyAPI.DTOs.Responses;

namespace PasabuyAPI.Services.Interfaces
{
    public interface IChatRoomService
    {
        Task<bool> CloseChatRoomAsync(long roomId, long currentUserId);
        Task<ChatRoomResponseDTO> GetChatRoomByOrderId(long orderId);
    }
}
