using Microsoft.AspNetCore.SignalR;
using PasabuyAPI.Data;

namespace PasabuyAPI.Hubs
{
    public class ChatHub(PasabuyDbContext context) : Hub
    {
        private readonly PasabuyDbContext _context = context;

        public async Task JoinRoom(long roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
        }
    }
}