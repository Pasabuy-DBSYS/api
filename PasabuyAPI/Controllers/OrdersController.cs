using Microsoft.AspNetCore.Mvc;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;

        [HttpGet]
        public async Task<ActionResult<List<OrderResponseDTO>>> GetAllOrdersAsync()
        {
            List<OrderResponseDTO> orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponseDTO>> GetOrderByIdAsync(long id)
        {
            OrderResponseDTO? response = await _orderService.GetOrderByOrderId(id);
            if (response == null) return NotFound($"Order id [{id}] not found");

            return Ok(response);
        }
        

        [HttpPost]
        public async Task<ActionResult<OrderResponseDTO>> CreateOrderAsync([FromBody] OrderRequestDTO orderRequest)
        {
            OrderResponseDTO response = await _orderService.CreateOrder(orderRequest);
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
    }
}