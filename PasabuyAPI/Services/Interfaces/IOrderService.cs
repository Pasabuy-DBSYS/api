using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Enums;
using PasabuyAPI.Models;

namespace PasabuyAPI.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderResponseDTO>> GetOrdersAsync();
        Task<OrderResponseDTO?> GetOrderByOrderId(long id);
        Task<OrderResponseDTO> CreateOrder(CreateOrderDTO orderData);
        Task<OrderResponseDTO> AcceptOrderAsync(DeliveryDetailsRequestDTO deliveryDetailsRequestDTO, long orderId, long courierId);
        Task<OrderResponseDTO> UpdateStatusAsync(long orderId, Status status);
        Task<List<OrderResponseDTO>> GetAllOrdersByStatus(Status status);
        Task<List<OrderResponseDTO>> GetAllOrdersByUserId(long userId);
    }
}