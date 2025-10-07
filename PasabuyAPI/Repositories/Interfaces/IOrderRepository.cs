using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Enums;
using PasabuyAPI.Models;

namespace PasabuyAPI.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Orders>> GetOrdersAsync();
        Task<Orders?> GetOrderByOrderId(long id);
        Task<Orders> CreateOrder(Orders orderData);
        Task<Orders> AcceptOrder(long orderId, long courierId, DeliveryDetails deliveryDetails);
        Task<Orders> UpdateStatusAsync(long orderId, Status status);
        Task<List<Orders>> GetAllOrdersByStatus(Status status);
        Task<List<Orders>> GetAllOrdersByCustomerId(long customerId);
        Task<List<Orders>> GetAllOrdersByCourierId(long courierId);

        //Helper Method
        Task<bool> IsUserAvailable(long userId);
    }
}