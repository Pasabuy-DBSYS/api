using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;

namespace PasabuyAPI.Services.Interfaces
{
    public interface IChatMessagesService
    {
        Task<ChatMessagesResponseDTO> SendMessage(SendMessageRequestDTO message);
        Task<List<ChatMessagesResponseDTO>> GetChatMessagesByRoomIdAsync(long roomId, long currentUserId);
    }
}