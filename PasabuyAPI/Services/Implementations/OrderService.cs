using Mapster;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Enums;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Services.Implementations;

public class OrderService(IOrderRepository orderRepository, IDeliveryDetailsRepository deliveryDetailsRepository, IPaymentsRepository paymentsRepository) : IOrderService
{
    public async Task<OrderResponseDTO> AcceptOrderAsync(DeliveryDetailsRequestDTO deliveryDetailsRequestDTO, long orderId, long courierId)
    {
        var deliveryDetails = deliveryDetailsRequestDTO.Adapt<DeliveryDetails>();
        var response = await orderRepository.AcceptOrder(orderId, courierId, deliveryDetails);

        return response.Adapt<OrderResponseDTO>();
    }


    public async Task<OrderResponseDTO> CreateOrder(CreateOrderDTO orderData)
    {
        Orders order = await orderRepository.CreateOrder(new()
        {
            CustomerId = orderData.CustomerId,
            Request = orderData.Request,
            Status = orderData.Status,
            Priority = orderData.Priority,
        });

        DeliveryDetails deliveryDetails = await deliveryDetailsRepository.CreateDeliveryDetails(new()
        {
            OrderIdFK = order.OrderIdPK,
            LocationLatitude = orderData.LocationLatitude,
            LocationLongitude = orderData.LocationLongitude,
            CustomerLatitude = orderData.CustomerLatitude,
            CustomerLongitude = orderData.CustomerLongitude,
            ActualDistance = orderData.DeliveryDistance,
            DeliveryNotes = orderData.DeliveryNotes
        });

        Payments payments = await paymentsRepository.CreatePayment(order.Priority, orderData.DeliveryDistance, new()
        {
            OrderIdFK = order.OrderIdPK,
            PaymentMethod = PaymentMethod.CASH,
            PaymentStatus = PaymentStatus.PENDING
        });

        var response = order.Adapt<OrderResponseDTO>();
        response.PaymentsResponseDTO = payments.Adapt<PaymentsResponseDTO>();
        response.DeliveryDetailsDTO = deliveryDetails.Adapt<DeliveryDetailsResponseDTO>();

        return response;
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
