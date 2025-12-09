using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Enums;
using PasabuyAPI.Exceptions;
using PasabuyAPI.Hubs;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Implementations;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController(IOrderService orderService, IHubContext<OrdersHub> hubContext, IHubContext<NotificationHub> notificationHub, INotificationService notificationService) : ControllerBase
    {
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<ActionResult<List<OrderResponseDTO>>> GetAllOrdersAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
                return Unauthorized("Invalid token — user ID not found.");

            if (!long.TryParse(userIdClaim, out var userId))
                return BadRequest("Invalid user ID format.");



            List<OrderResponseDTO> orders = await orderService.GetOrdersAsync();
            return Ok(orders);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponseDTO>> GetOrderByIdAsync(long id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
                return Unauthorized("Invalid token — user ID not found.");

            if (!long.TryParse(userIdClaim, out var userId))
                return BadRequest("Invalid user ID format.");

            OrderResponseDTO? response = await orderService.GetOrderByOrderId(id);
            if (response == null) return NotFound($"Order id [{id}] not found");

            return Ok(response);
        }

        [Authorize]
        [HttpGet("status/{status}")]
        public async Task<ActionResult<List<OrderResponseDTO>>> GetAllOrdersByStatus(Status status)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
                return Unauthorized("Invalid token — user ID not found.");

            if (!long.TryParse(userIdClaim, out var userId))
                return BadRequest("Invalid user ID format.");

            List<OrderResponseDTO> pendingOrders = await orderService.GetAllOrdersByStatus(status);

            return Ok(pendingOrders);
        }

        [Authorize]
        [HttpGet("history/customer")]
        public async Task<ActionResult<List<OrderResponseDTO>>> GetOrderHistoryByCustomerIdAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
                return Unauthorized("Invalid token — user ID not found.");

            if (!long.TryParse(userIdClaim, out var customerId))
                return BadRequest("Invalid user ID format.");

            List<OrderResponseDTO> ordersByUser = await orderService.GetAllOrdersByCustomerIdAsync(customerId);

            return Ok(ordersByUser);
        }

        [Authorize]
        [HttpGet("history/courier")]
        public async Task<ActionResult<List<OrderResponseDTO>>> GetOrderHistoryByCourierIdAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
                return Unauthorized("Invalid token — user ID not found.");

            if (!long.TryParse(userIdClaim, out var courierId))
                return BadRequest("Invalid user ID format.");

            List<OrderResponseDTO> ordersByUser = await orderService.GetAllOrdersByCourierIdAsync(courierId);

            return Ok(ordersByUser);
        }

        [Authorize(Policy = "CustomerOnly")]
        [HttpPost]
        public async Task<ActionResult<OrderResponseDTO>> CreateOrderAsync([FromBody] CreateOrderDTO orderRequest)
        {
            OrderResponseDTO response = await orderService.CreateOrder(orderRequest);

            await hubContext.Clients.Groups("COURIER").SendAsync("OrderCreated", response);

            return StatusCode(201, response);
        }

        [Authorize(Policy = "CourierOnly")]
        [HttpPost("accept/{orderId}")]
        public async Task<ActionResult<OrderResponseDTO>> AcceptOrderAsync([FromBody] AcceptOrderDTO acceptOrderDTO, long orderId)
        {
            var courierIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (long.TryParse(courierIdClaim, out var courierId))
                acceptOrderDTO.CourierId = courierId;
            else
                return BadRequest("Invalid courier ID in token.");

            OrderResponseDTO responseDTO = await orderService.AcceptOrderAsync(acceptOrderDTO, orderId);

            await hubContext.Clients.Group("COURIER").SendAsync("OrderAccepted", responseDTO);
            await hubContext.Clients.Group($"ORDER_{orderId}").SendAsync("OrderAccepted", responseDTO);

            NotificationRequestDTO notificationRequestDTO = new()
            {
                Title = "Order Accepted",
                Message = $"Order #{orderId} has been accepted.",
                Pressed = false,
                UserIdFk = responseDTO.CustomerId
            };

            var notificationResponse = await notificationService.CreateNotification(notificationRequestDTO);
            await notificationHub.Clients.Group($"user:{responseDTO.CustomerId}").SendAsync("ReceiveNotification", notificationResponse);

            return StatusCode(201, responseDTO);
        }

        [Authorize]
        [HttpPatch("update/{orderId}/{status}")]
        public async Task<ActionResult<OrderResponseDTO>> UpdateOrderStatusAsync(Status status, long orderId)
        {
            var currentUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(currentUser, out var currentUserId))
                return Forbid("Invalid token — user ID not found.");

            OrderResponseDTO responseDTO = await orderService.UpdateStatusAsync(orderId, status, currentUserId);

            await hubContext.Clients.Group($"ORDER_{orderId}").SendAsync("OrderStatusUpdated", responseDTO);

            // Create notification with custom message based on status
            var (title, message) = status switch
            {
                Status.ACCEPTED => ("Order Accepted", $"Order #{orderId} has been accepted by a courier"),
                Status.PICKED_UP => ("Order Picked Up", $"Your courier has picked up your order [Order #{orderId}]"),
                Status.IN_TRANSIT => ("Order In Transit", $"Order #{orderId} is on the way"),
                Status.DELIVERED => ("Order Delivered", $"Order #{orderId} has been delivered successfully"),
                Status.CANCELLED => ("Order Cancelled", $"Order #{orderId} has been cancelled"),
                _ => ("Order Status Updated", $"Order #{orderId} status has been updated to {status}")
            };

            NotificationRequestDTO notificationRequestDTO = new()
            {
                Title = title,
                Message = message,
                Pressed = false,
                UserIdFk = responseDTO.CustomerId
            };

            var notificationResponse = await notificationService.CreateNotification(notificationRequestDTO);
            await notificationHub.Clients.Group($"user:{responseDTO.CustomerId}").SendAsync("ReceiveNotification", notificationResponse);

            // If order is cancelled, notify the courier as well
            if (status == Status.CANCELLED && responseDTO.CourierId != 0)
            {
                NotificationRequestDTO courierNotificationDTO = new()
                {
                    Title = "Order Cancelled",
                    Message = $"Order #{orderId} has been cancelled",
                    Pressed = false,
                    UserIdFk = responseDTO.CourierId
                };

                var courierNotificationResponse = await notificationService.CreateNotification(courierNotificationDTO);
                await notificationHub.Clients.Group($"user:{responseDTO.CourierId}").SendAsync("ReceiveNotification", courierNotificationResponse);
            }

            return Ok(responseDTO);
        }

        [Authorize]
        [HttpGet("customer/activeorder")]
        public async Task<ActionResult<OrderResponseDTO>> GetActiveOrderCustomer()
        {
            var currentUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(currentUser, out var currentUserId))
                return Forbid("Invalid token - user ID not found");

            try
            {
                OrderResponseDTO activeOrder = await orderService.GetActiveOrderCustomer(currentUserId);
                return Ok(activeOrder);
            }
            catch (NotFoundException)
            {
                return NoContent();
            }
        }

        [Authorize]
        [HttpGet("courier/activeorder")]
        public async Task<ActionResult<OrderResponseDTO>> GetActiveOrderCourier()
        {
            var currentUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(currentUser, out var currentUserId))
                return Forbid("Invalid token - user ID not found");

            try
            {
                OrderResponseDTO activeOrder = await orderService.GetActiveOrderCourier(currentUserId);
                return Ok(activeOrder);
            }
            catch (NotFoundException)
            {
                return NoContent();
            }
        }

        [Authorize(Policy = "VerifiedOnly")]
        [HttpPatch("update/courier-location/{orderId}")]
        public async Task<IActionResult> ChangeCourierLocationByOrderId(long orderId, [FromQuery] long courierLatitude, [FromQuery] long courierLongitude)
        {
            var coords = new
            {
                courierLatitude,
                courierLongitude
            };

            await hubContext.Clients.Group($"ORDER_{orderId}").SendAsync("CourierLocationUpdated", coords);

            return Ok(coords);
        }

        [Authorize(Policy = "CourierOnly")]
        [HttpPatch("{orderId}/estimated-delivery-time")]
        public async Task<ActionResult<OrderResponseDTO>> UpdateEstimatedDeliveryTime(long orderId, [FromBody] UpdateEstimatedDeliveryTimeRequestDTO request)
        {
            var currentUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(currentUser, out var currentUserId))
                return Forbid("Invalid token - user ID not found");

            var response = await orderService.UpdateEstimatedDeliveryTime(orderId, request.EstimatedDeliveryTime, currentUserId);

            await hubContext.Clients.Group($"ORDER_{orderId}").SendAsync("EstimatedDeliveryTimeUpdated", new
            {
                orderId,
                response.DeliveryDetailsDTO.EstimatedDeliveryTime
            });

            return Ok(response);
        }

        [Authorize(Policy = "CourierOnly")]
        [HttpPatch("{orderId}/actual-pickup-time")]
        public async Task<ActionResult<OrderResponseDTO>> UpdateActualPickupTime(long orderId)
        {
            var currentUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(currentUser, out var currentUserId))
                return Forbid("Invalid token - user ID not found");

            var response = await orderService.UpdateActualPickupTime(orderId, currentUserId);

            await hubContext.Clients.Group($"ORDER_{orderId}").SendAsync("ActualPickupTimeUpdated", new
            {
                orderId,
                response.DeliveryDetailsDTO.ActualPickupTime
            });

            return Ok(response);
        }

        [Authorize(Policy = "CourierOnly")]
        [HttpPatch("{orderId}/actual-delivery-time")]
        public async Task<ActionResult<OrderResponseDTO>> UpdateActualDeliveryTime(long orderId)
        {
            var currentUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(currentUser, out var currentUserId))
                return Forbid("Invalid token - user ID not found");

            var response = await orderService.UpdateActualDeliveryTime(orderId, currentUserId);

            await hubContext.Clients.Group($"ORDER_{orderId}").SendAsync("ActualDeliveryTimeUpdated", new
            {
                orderId,
                response.DeliveryDetailsDTO.ActualDeliveryTime
            });

            return Ok(response);
        }
    }
}