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

        [HttpGet]
        public async Task<ActionResult<List<OrderResponseDTO>>> GetAllOrdersAsync()
        {
            List<OrderResponseDTO> orders = await orderService.GetOrdersAsync();
            return Ok(orders);
        }

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

        [Authorize(Policy = "CustomerOnly")]
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<List<OrderResponseDTO>>> GetAllOrdersByCustomerIdAsync(long customerId)
        {
            List<OrderResponseDTO> ordersByUser = await orderService.GetAllOrdersByCustomerIdAsync(customerId);

            return Ok(ordersByUser);
        }

        [Authorize(Policy = "CustomerOnly")]
        [HttpPost]
        public async Task<ActionResult<OrderResponseDTO>> CreateOrderAsync([FromBody] CreateOrderDTO orderRequest)
        {
            OrderResponseDTO response = await orderService.CreateOrder(orderRequest);

            await hubContext.Clients.All.SendAsync("OrderCreated", response);

            return CreatedAtAction(
                    nameof(GetOrderByIdAsync),
                    new { id = response.OrderIdPK },
                    new { response }
                );
        }
        
        [Authorize(Policy = "CouriersOnly")]
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

        [HttpPatch("update/{orderId}")]
        public async Task<ActionResult<OrderResponseDTO>> UpdateOrderStatusAsync([FromBody] Status status, long orderId)
        {
            OrderResponseDTO responseDTO = await orderService.UpdateStatusAsync(orderId, status);

            await hubContext.Clients.All.SendAsync("OrderStatusUpdated", responseDTO);

            return Ok(responseDTO);
        }
        
    }
}