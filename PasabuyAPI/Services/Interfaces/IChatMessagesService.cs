using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;

namespace PasabuyAPI.Services.Interfaces
{
    public interface IChatMessagesService
    {
        Task<ChatMessagesResponseDTO> SendTextMessage(SendMessageRequestDTO message);
        Task<ChatMessagesResponseDTO> SendImageMessage(SendImageMessageRequestDTO message);
        Task<List<ChatMessagesResponseDTO>> GetChatMessagesByRoomIdAsync(long roomId, long currentUserId);
    }
}