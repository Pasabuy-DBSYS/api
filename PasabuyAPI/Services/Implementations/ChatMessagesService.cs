using Mapster;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Services.Implementations
{
    public class ChatMessagesService(IChatMessagesRepository chatMessagesRepository) : IChatMessagesService
    {
        public async Task<List<ChatMessagesResponseDTO>> GetChatMessagesByRoomIdAsync(long roomId, long currentUserId)
        {
            List<ChatMessages> chatMessages = await chatMessagesRepository.GetChatMessagesByRoomIdAsync(roomId, currentUserId);

            return chatMessages.Adapt<List<ChatMessagesResponseDTO>>();
        }

        public async Task<ChatMessagesResponseDTO> SendMessage(SendMessageRequestDTO message)
        {
            var response = await chatMessagesRepository.SendMessage(message.Adapt<ChatMessages>());

            return response.Adapt<ChatMessagesResponseDTO>();
        }
    }
}