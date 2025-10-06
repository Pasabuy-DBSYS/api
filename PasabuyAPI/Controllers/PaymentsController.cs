using Microsoft.AspNetCore.Mvc;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController(IPaymentsService paymentsService) : ControllerBase
    {
        [HttpGet("{transactionId}")]
        public async Task<ActionResult<PaymentsResponseDTO>> GetPaymentsByTransactionId(string transactionId)
        {
            PaymentsResponseDTO responseDTO = await paymentsService.GetPaymentByTransactionId(transactionId);

            if (responseDTO is null) return NotFound($"Transcation Id {transactionId} not found");

            return Ok(responseDTO);
        }

        [HttpPost("propose/{orderId}")]
        public async Task<ActionResult<PaymentsResponseDTO>> ProposeItemsFeeAsync(long orderId, [FromBody] long itemsFee)
        {
            PaymentsResponseDTO? response = await paymentsService.ProposeItemsFeeAsync(orderId, itemsFee);

            if (response is null) return NotFound($"Order Id {orderId} is not found");

            return Ok(response);
        }

        [HttpPatch("propose/accept/{orderId}")]
        public async Task<ActionResult<PaymentsResponseDTO>> AcceptProposedItemsFeeAsync(long orderId)
        {
            PaymentsResponseDTO? responseDTO = await paymentsService.AcceptProposedItemsFeeAsync(orderId);
            
            if (responseDTO is null) return NotFound($"Order Id {orderId} is not found");

            return Ok(responseDTO);
        }
    }
}