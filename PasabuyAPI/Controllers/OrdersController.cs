using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Enums;
using PasabuyAPI.Hubs;
using PasabuyAPI.Models;
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
            List<OrderResponseDTO> orders = await orderService.GetOrdersAsync();
            return Ok(orders);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponseDTO>> GetOrderByIdAsync(long id)
        {
            OrderResponseDTO? response = await orderService.GetOrderByOrderId(id);
            if (response == null) return NotFound($"Order id [{id}] not found");

            return Ok(response);
        }

        [Authorize(Policy = "VerifiedOnly")]
        [HttpGet("status/{status}")]
        public async Task<ActionResult<List<OrderResponseDTO>>> GetAllOrdersByStatus(Status status)
        {
            List<OrderResponseDTO> pendingOrders = await orderService.GetAllOrdersByStatus(status);

            return Ok(pendingOrders);
        }

        [Authorize(Policy = "VerifiedOnly")]
        [HttpGet("customer")]
        public async Task<ActionResult<List<OrderResponseDTO>>> GetAllOrdersByCustomerIdAsync()
        {
            var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(customerIdClaim, out var customerId))
            {
                return BadRequest("Invalid courier ID in token.");
            }

            List<OrderResponseDTO> ordersByUser = await orderService.GetAllOrdersByCustomerIdAsync(customerId);

            return Ok(ordersByUser);
        }

        [Authorize(Policy = "VerifiedOnly")]
        [HttpGet("courier")]
        public async Task<ActionResult<List<OrderResponseDTO>>> GetAllOrdersByCourierIdAsync()
        {
            var courierIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(courierIdClaim, out var courierId))
            {
                return BadRequest("Invalid courier ID in token.");
            }

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
        
        [Authorize(Policy = "VerifiedOnly")]
        [HttpPatch("update/{orderId}/{status}")]
        public async Task<ActionResult<OrderResponseDTO>> UpdateOrderStatusAsync(Status status, long orderId)
        {
            var currentUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(!long.TryParse(currentUser, out var currentUserId))
            {
                return Unauthorized("Invalid token — user ID not found.");
            }

            OrderResponseDTO responseDTO = await orderService.UpdateStatusAsync(orderId, status, currentUserId);

            await hubContext.Clients.All.SendAsync("OrderStatusUpdated", responseDTO);

            return Ok(responseDTO);
        }
        
    }
}