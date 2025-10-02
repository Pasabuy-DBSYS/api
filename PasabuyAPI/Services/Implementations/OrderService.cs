using Mapster;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Enums;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Services.Implementations;

public class OrderService(IOrderRepository orderRepository) : IOrderService
{
    public async Task<OrderResponseDTO> AcceptOrderAsync(DeliveryDetailsRequestDTO deliveryDetailsRequestDTO, long orderId, long courierId)
    {
        var deliveryDetails = deliveryDetailsRequestDTO.Adapt<DeliveryDetails>();
        var response = await orderRepository.AcceptOrder(orderId, courierId, deliveryDetails);
        return response.Adapt<OrderResponseDTO>();
    }


    public async Task<OrderResponseDTO> CreateOrder(OrderRequestDTO orderData)
    {
        Orders order = await orderRepository.CreateOrder(orderData.Adapt<Orders>());
        return order.Adapt<OrderResponseDTO>();
    }

    public async Task<OrderResponseDTO?> GetOrderByOrderId(long id)
    {
        Orders? order = await orderRepository.GetOrderByOrderId(id);
        return order?.Adapt<OrderResponseDTO>();
    }

    public async Task<List<OrderResponseDTO>> GetOrdersAsync()
    {
        List<Orders> orders = await orderRepository.GetOrdersAsync();
        return orders.Adapt<List<OrderResponseDTO>>();
    }
    public async Task<OrderResponseDTO> UpdateStatusAsync(long orderId, Status status)
    {
        Orders order = await orderRepository.UpdateStatusAsync(orderId, status);
        return order.Adapt<OrderResponseDTO>();
    }

    public async Task<List<OrderResponseDTO>> GetAllOrdersByStatus(Status status)
    {
        List<Orders> orders = await orderRepository.GetAllOrdersByStatus(status);

        return orders.Adapt<List<OrderResponseDTO>>();
    }

    public async Task<List<OrderResponseDTO>> GetAllOrdersByUserId(long userId)
    {
        List<Orders> ordersByUserId = await orderRepository.GetAllOrdersByUserId(userId);

        return ordersByUserId.Adapt<List<OrderResponseDTO>>();
    }
}
