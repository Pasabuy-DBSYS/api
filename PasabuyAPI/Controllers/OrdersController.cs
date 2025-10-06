using Microsoft.AspNetCore.Mvc;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Enums;
using PasabuyAPI.Models;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController(IOrderService orderService) : ControllerBase
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

        [HttpGet("status/{status}")]
        public async Task<ActionResult<List<OrderResponseDTO>>> GetAllOrdersByStatus(Status status)
        {
            List<OrderResponseDTO> pendingOrders = await orderService.GetAllOrdersByStatus(status);

            return Ok(pendingOrders);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<OrderResponseDTO>>> GetAllOrdersByUserId(long userId)
        {
            List<OrderResponseDTO> ordersByUser = await orderService.GetAllOrdersByUserId(userId);

            return Ok(ordersByUser);
        }


        [HttpPost]
        public async Task<ActionResult<OrderResponseDTO>> CreateOrderAsync([FromBody] CreateOrderDTO orderRequest)
        {
            OrderResponseDTO response = await orderService.CreateOrder(orderRequest);
            return CreatedAtAction(
                    nameof(GetOrderByIdAsync),
                    new { id = response.OrderIdPK },
                    new
                    {
                        status = "created",
                        data = response
                    }
                );
        }

        [HttpPost("accept/{orderId}/{courierId}")]
        public async Task<ActionResult<OrderResponseDTO>> AcceptOrderAsync([FromBody] DeliveryDetailsRequestDTO deliveryDetailsRequestDTO, long orderId, long courierId)
        {
            OrderResponseDTO responseDTO = await orderService.AcceptOrderAsync(deliveryDetailsRequestDTO, orderId, courierId);

            return StatusCode(201, responseDTO);
        }

        [HttpPatch("update/{orderId}")]
        public async Task<ActionResult<OrderResponseDTO>> UpdateOrderStatusAsync([FromBody] Status status, long orderId)
        {
            OrderResponseDTO responseDTO = await orderService.UpdateStatusAsync(orderId, status);

            return Ok(responseDTO);
        }
        
    }
}