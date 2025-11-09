using Microsoft.EntityFrameworkCore;
using PasabuyAPI.Data;
using PasabuyAPI.Enums;
using PasabuyAPI.Exceptions;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;

namespace PasabuyAPI.Repositories.Implementations
{
    public class OrderRepository(PasabuyDbContext context) : IOrderRepository
    {
        private readonly PasabuyDbContext _context = context;
        private static readonly Status[] ActiveStatuses = [Status.PENDING, Status.ACCEPTED, Status.IN_TRANSIT];
        private static readonly Status[] CompletedStatuses = [Status.DELIVERED, Status.WATING_FOR_REVIEW, Status.REVIEWED];
        public async Task<List<Orders>> GetOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Courier)
                .Include(o => o.Customer)
                .Include(o => o.DeliveryDetails)
                .Include(o => o.Payment)
                .Include(o => o.ChatRoom)
                .ToListAsync();
        }
        public async Task<Orders?> GetOrderByOrderId(long id)
        {
            return await _context.Orders
                .Include(o => o.Courier)
                .Include(o => o.Customer)
                .Include(o => o.DeliveryDetails)
                .Include(o => o.Payment)
                .Include(o => o.ChatRoom)
                .FirstOrDefaultAsync(o => o.OrderIdPK == id);
        }
        public async Task<Orders> CreateOrder(Orders orderData)
        {
            if (!await IsUserAvailable(orderData.CustomerId)) throw new CannotCreateOrderException($"Customer Id {orderData.CustomerId} already has an active order");
            _context.Orders.Add(orderData);
            await _context.SaveChangesAsync();
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Courier)
                .FirstOrDefaultAsync(o => o.OrderIdPK == orderData.OrderIdPK)
                ?? orderData;
        }

        public async Task<Orders> AcceptOrder(long orderId, long courierId, DeliveryDetails deliveryDetails)
        {
            if (!await IsUserAvailable(courierId)) throw new CannotAcceptOrderException($"Courier id {courierId} already has an active order");
            // Load the existing order and its details (both tracked by EF)
            var trackedOrder = await _context.Orders
                .Include(o => o.DeliveryDetails)
                .Include(o => o.Payment)
                .Include(o => o.ChatRoom)
                .FirstOrDefaultAsync(o => o.OrderIdPK == orderId)
                ?? throw new Exception($"Order with ID {orderId} not found");

            if (trackedOrder.Status != Status.PENDING)
                throw new CannotAcceptOrderException($"Order Id {orderId} is not {Status.PENDING}");
                
            if (trackedOrder.CustomerId == courierId)
                throw new CannotAcceptOrderException($"Cannot accept own order");

            // Load courier
            var trackedCourier = await _context.Users
            .FirstOrDefaultAsync(u => u.UserIdPK == courierId)
            ?? throw new Exception($"Courier with ID {courierId} not found");

            // Update order info
            trackedOrder.Status = Status.ACCEPTED;
            trackedOrder.CourierId = trackedCourier.UserIdPK;
            trackedOrder.Courier = trackedCourier;

            // ✅ Update existing delivery details (DO NOT reassign or add new)
            var details = trackedOrder.DeliveryDetails;

            details.CourierLatitude = deliveryDetails.CourierLatitude;
            details.CourierLongitude = deliveryDetails.CourierLongitude;
            // Add any other fields that can chang

            var chatRoom = new ChatRooms
            {
                OrderIdFK = trackedOrder.OrderIdPK,
                Order = trackedOrder,
                IsActive = true
            };

            var trackedChatRoom = await _context.ChatRooms.AddAsync(chatRoom);

            trackedOrder.ChatRoom = chatRoom;

            // Save everything
            await _context.SaveChangesAsync();

            // Reload and return fully populated order
            return await _context.Orders
                .Include(o => o.Courier)
                .Include(o => o.Customer)
                .Include(o => o.DeliveryDetails)
                .Include(o => o.Payment)
                .Include(o => o.ChatRoom)
                .FirstOrDefaultAsync(o => o.OrderIdPK == orderId)
                ?? trackedOrder;
        }


        public async Task<Orders> UpdateStatusAsync(long orderId, Status status, long currentUserId)
        {
            var order = await _context.Orders
                            .Include(o => o.DeliveryDetails)
                            .Include(o => o.Payment)
                            .FirstOrDefaultAsync(o => o.OrderIdPK == orderId)
                            ?? throw new Exception($"Order id {orderId} not found");

            // Authorization check
            if (order.CustomerId != currentUserId && order.CourierId != currentUserId)
                throw new UnauthorizedAccessException("You are not authorized to update this order.");

            order.Updated_at = DateTime.UtcNow;
            order.Status = status;

            if (CompletedStatuses.Contains(status) && order.DeliveryDetails != null)
            {
                order.DeliveryDetails.ActualDeliveryTime = DateTime.UtcNow;
                order.Payment.PaidAt = DateTime.UtcNow;
                order.Payment.PaymentStatus = PaymentStatus.COMPLETED;
            }

            await _context.SaveChangesAsync();
            return order;
        }


        public async Task<List<Orders>> GetAllOrdersByStatus(Status status)
        {
            return await _context.Orders
                            .Where(o => o.Status == status) // 👈 filter only pending
                            .Include(o => o.Customer)               // optional: eager load related data
                            .Include(o => o.DeliveryDetails)        // optional: if you need delivery info
                            .Include(o => o.Payment)
                            .ToListAsync();
        }

        public async Task<List<Orders>> GetAllOrdersByCustomerId(long customerId)
        {
            return await _context.Orders
                            .Where(o => o.CustomerId == customerId)
                            .Include(o => o.Customer)               // optional: eager load related data
                            .Include(o => o.DeliveryDetails)        // optional: if you need delivery info
                            .Include(o => o.Payment)
                            .OrderBy(o => ActiveStatuses.Contains(o.Status) ? 0 : 1)
                            .ThenByDescending(o => o.Updated_at)
                            .ToListAsync();
        }

        public async Task<List<Orders>> GetAllOrdersByCourierId(long courierId)
        {
            return await _context.Orders
                            .Where(o => o.CourierId == courierId)
                            .Include(o => o.Customer)
                            .Include(o => o.Courier)
                            .Include(o => o.DeliveryDetails)
                            .Include(o => o.Payment)
                            .OrderBy(o => ActiveStatuses.Contains(o.Status) ? 0 : 1)
                            .ThenByDescending(o => o.Updated_at)
                            .ToListAsync();
        }

        // Helper Methods
        public async Task<bool> IsUserAvailable(long userId)
        {
            bool hasActiveOrder = await _context.Orders.AnyAsync(o =>
                ActiveStatuses.Contains(o.Status) &&
                (o.CourierId == userId || o.CustomerId == userId));

            return !hasActiveOrder;
        }
    }
}
