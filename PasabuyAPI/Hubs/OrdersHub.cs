using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PasabuyAPI.Data;

namespace PasabuyAPI.Hubs
{
    [Authorize(Policy = "VerifiedOnly")]
    public class OrdersHub(PasabuyDbContext context) : Hub
    {
        private readonly PasabuyDbContext _context = context;
        
        public override async Task OnConnectedAsync()
        {
            // optional: log or track user connection
            await base.OnConnectedAsync();
        }
        // ========================
        // JOIN GROUPS
        // ========================

         public async Task JoinCourierGroup()
        {
            if (!Context.User!.IsInRole("COURIER"))
                throw new HubException("You are not authorized to join the courier group.");

            await Groups.AddToGroupAsync(Context.ConnectionId, "COURIER");
        }

        public async Task JoinCustomerGroup()
        {
            if (!Context.User!.IsInRole("CUSTOMER"))
                throw new HubException("You are not authorized to join the customer group.");

            await Groups.AddToGroupAsync(Context.ConnectionId, "CUSTOMER");
        }


        public async Task JoinOrderGroup(long orderId)
        {
            var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(userIdClaim, out var userId))
                throw new HubException("Invalid user identity.");

            // Load the order + related data (if needed)
            var order = await _context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.OrderIdPK == orderId)
                ?? throw new HubException("Order not found.");

            // allowed participants: customer and courier
            var allowedUserIds = new List<long>
            {
                order.CustomerId,
                order.CourierId ?? 0
            };

            if (!allowedUserIds.Contains(userId))
                throw new HubException("You are not authorized to join this order.");

            // Add the connection to the group (ORDER_12345)
            await Groups.AddToGroupAsync(Context.ConnectionId, $"ORDER_{orderId}");
        }

        // ========================
        // LEAVE GROUPS
        // ========================

        public async Task LeaveCourierGroup()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "COURIER");
        }

        public async Task LeaveCustomerGroup()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "CUSTOMER");
        }

        public async Task LeaveOrderGroup(string orderId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"ORDER_{orderId}");
        }

        // ========================
        // COURIER LOCATION UPDATE
        // ========================

        public async Task UpdateCourierLocation(long orderId, decimal courierLatitude, decimal courierLongitude)
        {
            // Verify user is a courier
            if (!Context.User!.IsInRole("COURIER"))
                throw new HubException("Only couriers can send location updates.");

            var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(userIdClaim, out var userId))
                throw new HubException("Invalid user identity.");

            // Verify the courier is assigned to this order
            var order = await _context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.OrderIdPK == orderId)
                ?? throw new HubException("Order not found.");

            if (order.CourierId != userId)
                throw new HubException("You are not authorized to send location updates for this order.");

            // Always broadcast via SignalR for real-time tracking
            var coords = new
            {
                orderId,
                courierLatitude,
                courierLongitude
            };

            await Clients.Group($"ORDER_{orderId}").SendAsync("CourierLocationUpdated", coords);

            // Update database every 30 seconds (throttled)
            var deliveryDetails = await _context.DeliveryDetails
                .FirstOrDefaultAsync(d => d.OrderIdFK == orderId);

            if (deliveryDetails != null)
            {
                // Check if 30 seconds have passed since last update
                var timeSinceLastUpdate = DateTime.UtcNow - deliveryDetails.UpdatedAt;
                
                if (timeSinceLastUpdate.TotalSeconds >= 30)
                {
                    deliveryDetails.CourierLatitude = courierLatitude;
                    deliveryDetails.CourierLongitude = courierLongitude;
                    deliveryDetails.UpdatedAt = DateTime.UtcNow;
                    
                    await _context.SaveChangesAsync();
                }
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // optional: handle cleanup logic if needed
            await base.OnDisconnectedAsync(exception);
        }
    }
}
