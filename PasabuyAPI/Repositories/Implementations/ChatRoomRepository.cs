using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PasabuyAPI.Data;
using PasabuyAPI.Repositories.Interfaces;

namespace PasabuyAPI.Repositories.Implementations
{
    public class ChatRoomRepository(PasabuyDbContext context) : IChatRoomRepository
    {
        public async Task<bool> CloseChatRoomAsync(long roomId, long currentUserId)
        {
            var chatRoom = await context.ChatRooms
                .Include(r => r.Order)
                .FirstOrDefaultAsync(r => r.RoomIdPK == roomId) ?? throw new Exception("Chat room not found");

            var allowedUserIds = new List<long>
            {
                chatRoom.Order.CourierId ?? 0,
                chatRoom.Order.CustomerId
            };

            chatRoom.IsActive = false;
            chatRoom.ClosedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();
            return true;
        }
    }
}