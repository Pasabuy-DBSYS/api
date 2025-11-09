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
    public class OrdersController(IOrderService orderService, IHubContext<OrdersHub> hubContext) : ControllerBase
    {
        [Authorize]
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

            await hubContext.Clients.All.SendAsync("OrderCreated", response);

            return StatusCode(201, response);
        }

        [Authorize(Policy = "CourierOnly")]
        [HttpPost("accept/{orderId}")]
        public async Task<ActionResult<OrderResponseDTO>> AcceptOrderAsync([FromBody] AcceptOrderDTO acceptOrderDTO, long orderId)
        {
            var courierIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (long.TryParse(courierIdClaim, out var courierId))
            {
                acceptOrderDTO.CourierId = courierId;
            }
            else
            {
                return BadRequest("Invalid courier ID in token.");
            }

            OrderResponseDTO responseDTO = await orderService.AcceptOrderAsync(acceptOrderDTO, orderId);

            await hubContext.Clients.All.SendAsync("OrderAccepted", responseDTO);

            return StatusCode(201, responseDTO);
        }

        [Authorize]
        [HttpPatch("update/{orderId}/{status}")]
        public async Task<ActionResult<OrderResponseDTO>> UpdateOrderStatusAsync(Status status, long orderId)
        {
            var currentUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(currentUser, out var currentUserId))
            {
                return Forbid("Invalid token — user ID not found.");
            }

            OrderResponseDTO responseDTO = await orderService.UpdateStatusAsync(orderId, status, currentUserId);

            await hubContext.Clients.All.SendAsync("OrderStatusUpdated", responseDTO);

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
        
    }
}