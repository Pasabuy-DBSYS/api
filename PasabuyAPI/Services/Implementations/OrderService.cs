using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Services.Implementations
{
    public class OrderService(IOrderRepository orderRepository) : IOrderService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<OrderResponseDTO> CreateOrder(OrderRequestDTO orderData)
        {
            Orders order = await _orderRepository.CreateOrder(orderData.Adapt<Orders>());
            return order.Adapt<OrderResponseDTO>();

        }

        public async Task<OrderResponseDTO?> GetOrderByOrderId(long id)
        {
            Orders? order = await _orderRepository.GetOrderByOrderId(id);
            return order.Adapt<OrderResponseDTO>();
        }

        public async Task<List<OrderResponseDTO>> GetOrdersAsync()
        {
            List<Orders> orders = await _orderRepository.GetOrdersAsync();

            return orders.Adapt<List<OrderResponseDTO>>();
        }
    }
}