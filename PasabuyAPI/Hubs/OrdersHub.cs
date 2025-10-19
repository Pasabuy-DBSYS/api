using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace PasabuyAPI.Hubs
{
    [Authorize(Policy = "VerifiedOnly")]
    public class OrdersHub : Hub
    {
        
    }
}