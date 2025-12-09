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
        Task<Orders> UpdateStatusAsync(long orderId, Status status, long currentUserId);
        Task<List<Orders>> GetAllOrdersByStatus(Status status);
        Task<List<Orders>> GetAllOrdersByCustomerId(long customerId);
        Task<List<Orders>> GetAllOrdersByCourierId(long courierId);
        Task<Orders> GetActiveOrderCustomer(long customerId);
        Task<Orders> GetActiveOrderCourier(long courierId);
        Task<Orders> UpdateCustomerReviewedStatus(long customerId, bool status, long orderId);
        Task<Orders> UpdateCourierReviewedStatus(long courierId, bool status, long orderId);
        Task<Orders> UpdateEstimatedDeliveryTime(long orderId, DateTime estimatedDeliveryTime);
        Task<Orders> UpdateActualDeliveryTime(long orderId);
        Task<Orders> UpdateActualPickupTime(long orderId);

        //Helper Method
        Task<bool> IsUserAvailable(long userId);
        Task<int> GetTotalDeliveries(long courierId);
        Task<int> GetTotalOrders(long customerId);
    }
}