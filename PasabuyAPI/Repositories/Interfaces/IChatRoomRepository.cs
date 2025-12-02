using PasabuyAPI.Models;

namespace PasabuyAPI.Repositories.Interfaces
{
    public interface IChatRoomRepository
    {
        Task<bool> CloseChatRoomAsync(long roomId, long currentUserId);
        Task<ChatRooms> GetChatRoomByOrderId(long orderId);
    }
}