using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Models;

namespace PasabuyAPI.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderResponseDTO>> GetOrdersAsync();
        Task<OrderResponseDTO?> GetOrderByOrderId(long id);
        Task<OrderResponseDTO> CreateOrder(OrderRequestDTO orderData);
    }
}