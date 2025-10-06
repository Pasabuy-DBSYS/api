using Microsoft.EntityFrameworkCore;
using PasabuyAPI.Data;
using PasabuyAPI.Enums;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;

namespace PasabuyAPI.Repositories.Implementations
{
    public class OrderRepository(PasabuyDbContext context) : IOrderRepository
    {
        private readonly PasabuyDbContext _context = context;
        public async Task<List<Orders>> GetOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Courier)
                .Include(o => o.Customer)
                .Include(o => o.DeliveryDetails)
                .Include(o => o.Payment)
                .ToListAsync();
        }
        public async Task<Orders?> GetOrderByOrderId(long id)
        {
            return await _context.Orders
                .Include(o => o.Courier)
                .Include(o => o.Customer)
                .Include(o => o.DeliveryDetails)
                .Include(o => o.Payment)
                .FirstOrDefaultAsync(o => o.OrderIdPK == id);
        }
        public async Task<Orders> CreateOrder(Orders orderData)
        {
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
            // Load the existing order from the database (tracked by EF)
            var trackedOrder = await _context.Orders
                .Include(o => o.DeliveryDetails)
                .FirstOrDefaultAsync(o => o.OrderIdPK == orderId)
                ?? throw new Exception($"Order with ID {orderId} not found");

            if (trackedOrder.Status != Status.PENDING) throw new Exception($"Cannot accept order");

            // Load the courier from Users table
            var trackedCourier = await _context.Users
                .FirstOrDefaultAsync(u => u.UserIdPK == courierId)
                ?? throw new Exception($"Courier with ID {courierId} not found");

            // Update Order Status
            trackedOrder.Status = Enums.Status.ACCEPTED;

            // Update courier fields
            trackedOrder.CourierId = trackedCourier.UserIdPK;
            trackedOrder.Courier = trackedCourier;

            // Attach new delivery details
            deliveryDetails.OrderIdFK = trackedOrder.OrderIdPK;
            trackedOrder.DeliveryDetails = deliveryDetails;
            _context.DeliveryDetails.Add(deliveryDetails);

            // Save changes
            await _context.SaveChangesAsync();

            return await _context.Orders
                .Include(o => o.Courier)
                .Include(o => o.Customer)
                .Include(o => o.DeliveryDetails)
                .Include(o => o.Payment)
                .FirstOrDefaultAsync(o => o.OrderIdPK == orderId)
                ?? trackedOrder;
        }

        public async Task<Orders> UpdateStatusAsync(long orderId, Status status)
        {
            var order = await _context.Orders
                        .Include(o => o.DeliveryDetails)  // 👈 make sure DeliveryDetails is loaded
                        .FirstOrDefaultAsync(o => o.OrderIdPK == orderId)
                        ?? throw new Exception($"Order id {orderId} not found");

            order.Updated_at = DateTime.UtcNow;
            order.Status = status;

            if (status == Status.DELIVERED && order.DeliveryDetails != null)
            {
                order.DeliveryDetails.ActualDeliveryTime = DateTime.UtcNow;
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

        public async Task<List<Orders>> GetAllOrdersByUserId(long userId)
        {
            return await _context.Orders
                            .Where(o => o.CustomerId == userId)
                            .Include(o => o.Customer)               // optional: eager load related data
                            .Include(o => o.DeliveryDetails)        // optional: if you need delivery info
                            .Include(o => o.Payment)
                            .ToListAsync();
        }
    }
}