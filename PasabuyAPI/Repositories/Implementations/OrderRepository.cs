using Microsoft.EntityFrameworkCore;
using PasabuyAPI.Data;
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
                .ToListAsync();
        }
        public async Task<Orders?> GetOrderByOrderId(long id)
        {
            return await _context.Orders
                .Include(o => o.Courier)
                .Include(o => o.Customer)
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
    }
}