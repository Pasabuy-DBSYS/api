using Microsoft.EntityFrameworkCore;
using PasabuyAPI.Data;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;

namespace PasabuyAPI.Repositories.Implementations
{
    public class ChatMessagesRepository(PasabuyDbContext context) : IChatMessagesRepository
    {
        public async Task<ChatMessages> SendMessage(ChatMessages message)
        {
            // Validate chat room
            var chatRoom = await context.ChatRooms
                .Include(r => r.Order)
                .FirstOrDefaultAsync(r => r.RoomIdPK == message.RoomIdFK)
                ?? throw new Exception("Chat room not found.");

            var order = chatRoom.Order;

            // Determine allowed users (customer & courier)
            var allowedUserIds = new List<long>
            {
                order.CustomerId,
                order.CourierId ?? 0
            };

            // Check if sender is part of the chat
            if (!allowedUserIds.Contains(message.SenderIdFK))
                throw new UnauthorizedAccessException("You are not allowed to send messages in this chat.");

            // Optional: Check if receiver is valid (prevent spoofing)
            if (!allowedUserIds.Contains(message.ReceiverIdFK))
                throw new UnauthorizedAccessException("Invalid receiver for this chat room.");

            await context.ChatMessages.AddAsync(message);
            await context.SaveChangesAsync();

            return message;
        }

        public async Task<List<ChatMessages>> GetChatMessagesByRoomIdAsync(long roomId, long currentUserId)
        {
            var chatRoom = await context.ChatRooms
                .Include(r => r.Order)
                .FirstOrDefaultAsync(r => r.RoomIdPK == roomId) ?? throw new Exception("Chat room not found");

            // Get the participants (customer & rider) for this order
            var order = chatRoom.Order;

            var allowedUserIds = new List<long>
            {
                order.CustomerId,       // customer
                order.CourierId ?? 0    // courier
            };

            // Check if current user is one of them
            if (!allowedUserIds.Contains(currentUserId))
                throw new UnauthorizedAccessException("You are not allowed to access this chat.");
        
            return await context.ChatMessages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => m.RoomIdFK == roomId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }
    }
}