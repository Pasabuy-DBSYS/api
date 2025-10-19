using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PasabuyAPI.Data;

namespace PasabuyAPI.Hubs
{
    [Authorize(Policy = "VerifiedOnly")]
    public class ChatHub(PasabuyDbContext context) : Hub
    {
        private readonly PasabuyDbContext _context = context;

        public override async Task OnConnectedAsync()
        {
            // optional: log or track user connection
            await base.OnConnectedAsync();
        }

        public async Task JoinRoom(long roomId)
        {
            var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(userIdClaim, out var userId))
                throw new HubException("Invalid user identity.");

            // validate the chat room
            var chatRoom = await _context.ChatRooms
                .Include(r => r.Order)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RoomIdPK == roomId) ?? throw new HubException("Chat room not found.");
            
            // Validate the order
            var order = chatRoom.Order 
                ?? throw new HubException("Associated order not found for this chat room.");

            // allowed participants
            var allowedUserIds = new List<long>
            {
                order.CustomerId,
                order.CourierId ?? 0
            };

            if (!allowedUserIds.Contains(userId))
                throw new HubException("You are not authorized to join this chat.");

            // add connection to group
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // optional: handle cleanup logic if needed
            await base.OnDisconnectedAsync(exception);
        }
    }
}
