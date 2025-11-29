using Microsoft.AspNetCore.SignalR;

namespace PasabuyAPI.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task JoinUserGroup(long userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");
        }

        public async Task LeaveUserGroup(long userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user:{userId}");
        }
    }
}