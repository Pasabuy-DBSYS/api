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
        private static readonly Status[] ActiveStatuses = [Status.PENDING, Status.ACCEPTED, Status.PICKED_UP, Status.IN_TRANSIT];
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
            // Begin Transaction
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _context.Orders
                                .Include(o => o.DeliveryDetails)
                                .Include(o => o.Payment)
                                .Include(o => o.ChatRoom)
                                .FirstOrDefaultAsync(o => o.OrderIdPK == orderId)
                                ?? throw new Exception($"Order id {orderId} not found");

                // Authorization check
                if (order.CustomerId != currentUserId && order.CourierId != currentUserId)
                    throw new UnauthorizedAccessException("You are not authorized to update this order.");

                // Store the original status BEFORE updating the order
                var originalStatus = order.Status;

                // CRITICAL CANCELLATION CHECK
                if (status == Status.CANCELLED)
                {
                    order.Payment.PaymentStatus = PaymentStatus.CANCELLED;
                    // Only perform the time check if the order was previously active.
                    if (ActiveStatuses.Contains(originalStatus))
                    {
                        // Check if the order is older than 5 minutes
                        if (order.Created_at < DateTime.UtcNow.AddMinutes(-5))
                        {
                            throw new Exception("Cannot cancel order now: The 5-minute grace period for active orders has expired.");
                        }
                    }

                    // Apply cancellation-specific updates
                    if (order.ChatRoom != null)
                    {
                        order.ChatRoom.ClosedAt = DateTime.UtcNow;
                        order.ChatRoom.IsActive = false;
                    }
                }

                // Apply general status updates
                order.Updated_at = DateTime.UtcNow;
                order.Status = status;

                if (status == Status.DELIVERED && order.DeliveryDetails != null)
                {
                    order.DeliveryDetails.ActualDeliveryTime = DateTime.UtcNow;
                    order.Payment.PaidAt = DateTime.UtcNow;
                    order.Payment.PaymentStatus = PaymentStatus.COMPLETED;
                    order.ChatRoom.ClosedAt = DateTime.UtcNow;
                    order.ChatRoom.IsActive = false;

                    var courier = await _context.Users.FirstOrDefaultAsync(u => u.UserIdPK == order.CourierId);
                    courier.TotalDeliveries += 1;

                    var customer = await _context.Users.FirstOrDefaultAsync(u => u.UserIdPK == order.CustomerId);
                    customer.TotalOrders += 1;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<List<Orders>> GetAllOrdersByStatus(Status status)
        {
            IQueryable<Orders> query = _context.Orders
                .Where(o => o.Status == status)
                .Include(o => o.Customer)
                .Include(o => o.DeliveryDetails)
                .Include(o => o.Payment);

            if (status == Status.PENDING)
                query = query.OrderByDescending(o => o.Priority == Priority.URGENT);

            return await query.ToListAsync();
        }

        public async Task<List<Orders>> GetAllOrdersByCustomerId(long customerId) // Get all past order
        {
            return await _context.Orders
                            .Where(o => o.CustomerId == customerId)
                            .Include(o => o.Customer)               // optional: eager load related data
                            .Include(o => o.DeliveryDetails)        // optional: if you need delivery info
                            .Include(o => o.Payment)
                            .Where(o => !ActiveStatuses.Contains(o.Status))
                            .OrderByDescending(o => o.Updated_at)
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
                            .Where(o => !ActiveStatuses.Contains(o.Status))
                            .OrderByDescending(o => o.Updated_at)
                            .ToListAsync();
        }

        public async Task<Orders> GetActiveOrderCustomer(long customerId)
        {
            return await _context.Orders
                            .Include(o => o.Customer)
                            .Include(o => o.Courier)
                            .Include(o => o.DeliveryDetails)
                            .Include(o => o.Payment)
                            .FirstOrDefaultAsync(o => o.CustomerId == customerId && ActiveStatuses.Contains(o.Status))
                            ?? throw new NotFoundException($"There is no active order for customer {customerId}");

        }

        public async Task<Orders> GetActiveOrderCourier(long courierId)
        {
            return await _context.Orders
                            .Include(o => o.Customer)
                            .Include(o => o.Courier)
                            .Include(o => o.DeliveryDetails)
                            .Include(o => o.Payment)
                            .FirstOrDefaultAsync(o => o.CourierId == courierId && ActiveStatuses.Contains(o.Status)) ?? throw new NotFoundException($"There is no active order for customer {courierId}");

        }

        // Helper Methods
        public async Task<bool> IsUserAvailable(long userId)
        {
            bool hasActiveOrder = await _context.Orders.AnyAsync(o =>
                ActiveStatuses.Contains(o.Status) &&
                (o.CourierId == userId || o.CustomerId == userId));

            return !hasActiveOrder;
        }

        public async Task<Orders> UpdateCustomerReviewedStatus(long customerId, bool status, long orderId)
        {
            var order = await _context.Orders
                                .Include(o => o.Customer)
                                .Include(o => o.Courier)
                                .Include(o => o.DeliveryDetails)
                                .Include(o => o.Payment)
                                .FirstOrDefaultAsync(o => o.OrderIdPK == orderId && o.CustomerId == customerId)
                                    ?? throw new NotFoundException($"Combination of Order ID {orderId} and Customer ID {customerId} does not exist");

            order.IsCustomerReviewed = status;
            order.Updated_at = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Orders> UpdateCourierReviewedStatus(long courierId, bool status, long orderId)
        {
            var order = await _context.Orders
                                .Include(o => o.Customer)
                                .Include(o => o.Courier)
                                .Include(o => o.DeliveryDetails)
                                .Include(o => o.Payment)
                                .FirstOrDefaultAsync(o => o.OrderIdPK == orderId && o.CourierId == courierId)
                                    ?? throw new NotFoundException($"Combination of Order ID {orderId} and Courier ID {courierId} does not exist");

            order.IsCourierReviewed = status;
            order.Updated_at = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<int> GetTotalDeliveries(long courierId)
        {
            return await _context.Orders.CountAsync(o => o.CourierId == courierId && o.Status == Status.DELIVERED);
        }

        public async Task<int> GetTotalOrders(long customerId)
        {
            return await _context.Orders.CountAsync(o => o.CustomerId == customerId && o.Status == Status.DELIVERED);
        }
    }
}
