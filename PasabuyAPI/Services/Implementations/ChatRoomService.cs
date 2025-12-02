using Mapster;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Services.Implementations
{
    public class ChatRoomService(IChatRoomRepository chatRoomRepository) : IChatRoomService
    {
        public async Task<bool> CloseChatRoomAsync(long roomId, long currentUserId)
        {
            return await chatRoomRepository.CloseChatRoomAsync(roomId, currentUserId);
        }

        public async Task<ChatRoomResponseDTO> GetChatRoomByOrderId(long orderId)
        {
            var response = await chatRoomRepository.GetChatRoomByOrderId(orderId);
            return response.Adapt<ChatRoomResponseDTO>();
        }
    }
}
