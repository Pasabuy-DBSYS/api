using Mapster;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Enums;
using PasabuyAPI.Models;
using PasabuyAPI.Exceptions;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Services.Implementations;

public class OrderService(IOrderRepository orderRepository, IDeliveryDetailsRepository deliveryDetailsRepository, IPaymentsRepository paymentsRepository) : IOrderService
{
    public async Task<OrderResponseDTO> AcceptOrderAsync(AcceptOrderDTO acceptOrderDTO, long orderId)
    {
        var response = await orderRepository.AcceptOrder(orderId, acceptOrderDTO.CourierId, new()
        {
            CourierLatitude = acceptOrderDTO.CourierLatitude,
            CourierLongitude = acceptOrderDTO.CourierLongitude
        });

        return response.Adapt<OrderResponseDTO>();
    }


    public async Task<OrderResponseDTO> CreateOrder(CreateOrderDTO orderData)
    {
        Orders order = await orderRepository.CreateOrder(new()
        {
            CustomerId = orderData.CustomerId,
            Request = orderData.Request,
            Status = Status.PENDING,
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
            DeliveryNotes = orderData.DeliveryNotes,
            CustomerAddress = orderData.CustomerAddress,
            DestinationAddress = orderData.DestinationAddress
        });

        Payments payments = await paymentsRepository.CreatePayment(order.Priority, orderData.DeliveryDistance, new()
        {
            OrderIdFK = order.OrderIdPK,
            PaymentMethod = PaymentMethod.CASH,
            PaymentStatus = PaymentStatus.PENDING,
            TipAmount = orderData.TipFee
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
    public async Task<OrderResponseDTO> UpdateStatusAsync(long orderId, Status status, long currentUserId)
    {
        Orders order = await orderRepository.UpdateStatusAsync(orderId, status, currentUserId);
        return order.Adapt<OrderResponseDTO>();
    }

    public async Task<List<OrderResponseDTO>> GetAllOrdersByStatus(Status status)
    {
        List<Orders> orders = await orderRepository.GetAllOrdersByStatus(status);

        return orders.Adapt<List<OrderResponseDTO>>();
    }

    public async Task<List<OrderResponseDTO>> GetAllOrdersByCustomerIdAsync(long customerId)
    {
        List<Orders> ordersByCustomerId = await orderRepository.GetAllOrdersByCustomerId(customerId);

        return ordersByCustomerId.Adapt<List<OrderResponseDTO>>();
    }

    public async Task<List<OrderResponseDTO>> GetAllOrdersByCourierIdAsync(long courierId)
    {
        List<Orders> ordersByCourierId = await orderRepository.GetAllOrdersByCourierId(courierId);

        return ordersByCourierId.Adapt<List<OrderResponseDTO>>();
    }

    public async Task<OrderResponseDTO> GetActiveOrderCustomer(long customerId)
    {
        Orders activeOrder = await orderRepository.GetActiveOrderCustomer(customerId);

        return activeOrder.Adapt<OrderResponseDTO>();
    }
    public async Task<OrderResponseDTO> GetActiveOrderCourier(long courierId)
    {
        Orders activeOrder = await orderRepository.GetActiveOrderCourier(courierId);

        return activeOrder.Adapt<OrderResponseDTO>();
    }

    public async Task<OrderResponseDTO> UpdateCustomerReviewedStatus(long customerId, bool status, long orderId)
    {
        Orders updated = await orderRepository.UpdateCustomerReviewedStatus(customerId, status, orderId);
        return updated.Adapt<OrderResponseDTO>();
    }

    public async Task<OrderResponseDTO> UpdateCourierReviewedStatus(long courierId, bool status, long orderId)
    {
        Orders updated = await orderRepository.UpdateCourierReviewedStatus(courierId, status, orderId);
        return updated.Adapt<OrderResponseDTO>();
    }

    public async Task<OrderResponseDTO> UpdateEstimatedDeliveryTime(long orderId, DateTime estimatedDeliveryTime, long currentUserId)
    {
        var order = await orderRepository.GetOrderByOrderId(orderId) ?? throw new NotFoundException($"Order id {orderId} not found");
        EnsureCourierAccess(order, currentUserId);

        Orders updated = await orderRepository.UpdateEstimatedDeliveryTime(orderId, estimatedDeliveryTime);
        return updated.Adapt<OrderResponseDTO>();
    }

    public async Task<OrderResponseDTO> UpdateActualDeliveryTime(long orderId, long currentUserId)
    {
        var order = await orderRepository.GetOrderByOrderId(orderId) ?? throw new NotFoundException($"Order id {orderId} not found");
        EnsureCourierAccess(order, currentUserId);

        Orders updated = await orderRepository.UpdateActualDeliveryTime(orderId);
        return updated.Adapt<OrderResponseDTO>();
    }

    public async Task<OrderResponseDTO> UpdateActualPickupTime(long orderId, long currentUserId)
    {
        var order = await orderRepository.GetOrderByOrderId(orderId) ?? throw new NotFoundException($"Order id {orderId} not found");
        EnsureCourierAccess(order, currentUserId);

        Orders updated = await orderRepository.UpdateActualPickupTime(orderId);
        return updated.Adapt<OrderResponseDTO>();
    }

    private static void EnsureCourierAccess(Orders order, long currentUserId)
    {
        if (order.CourierId == null || order.CourierId != currentUserId)
            throw new UnauthorizedAccessException("You are not authorized to update this order.");
    }
}
