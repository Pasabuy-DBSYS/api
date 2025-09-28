using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.Models;

namespace PasabuyAPI.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Orders>> GetOrdersAsync();
        Task<Orders?> GetOrderByOrderId(long id);
        Task<Orders> CreateOrder(Orders orderData);
    }
}