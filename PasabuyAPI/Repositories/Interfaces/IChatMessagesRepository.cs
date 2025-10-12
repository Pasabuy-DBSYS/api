using PasabuyAPI.Models;

namespace PasabuyAPI.Repositories.Interfaces
{
    public interface IChatMessagesRepository
    {
        Task<ChatMessages> SendMessage(ChatMessages message);
        Task<List<ChatMessages>> GetChatMessagesByRoomIdAsync(long roomId, long currentUserId);
    }
}