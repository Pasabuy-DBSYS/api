using Mapster;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Enums;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Services.Implementations
{
    public class ChatMessagesService(IChatMessagesRepository chatMessagesRepository, IAwsS3Service awsS3Service) : IChatMessagesService
    {
        public async Task<List<ChatMessagesResponseDTO>> GetChatMessagesByRoomIdAsync(long roomId, long currentUserId)
        {
            List<ChatMessages> chatMessages = await chatMessagesRepository.GetChatMessagesByRoomIdAsync(roomId, currentUserId);

            return chatMessages.Adapt<List<ChatMessagesResponseDTO>>();
        }

        public async Task<ChatMessagesResponseDTO> SendTextMessage(SendMessageRequestDTO message)
        {
            message.MessageType = MessageTypes.TEXT;
            var response = await chatMessagesRepository.SendMessage(message.Adapt<ChatMessages>());
            return response.Adapt<ChatMessagesResponseDTO>();
        }

        public async Task<ChatMessagesResponseDTO> SendImageMessage(SendImageMessageRequestDTO message)
        {
            string key = $"messages/{Guid.NewGuid()}";
            string path = await awsS3Service.UploadFileAsync(message.Image, key);
            message.Message = path;
            message.MessageType = MessageTypes.IMAGE;

            var response = await chatMessagesRepository.SendMessage(message.Adapt<ChatMessages>());
            return response.Adapt<ChatMessagesResponseDTO>();
        }
    }
}