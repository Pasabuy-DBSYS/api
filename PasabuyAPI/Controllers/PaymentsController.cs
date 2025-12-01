using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Hubs;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController(IPaymentsService paymentsService, IHubContext<OrdersHub> orderHub) : ControllerBase
    {
        [Authorize(Policy = "VerifiedOnly")]
        [HttpGet("transaction/{transactionId}")]
        public async Task<ActionResult<PaymentsResponseDTO>> GetPaymentsByTransactionId(string transactionId)
        {
            PaymentsResponseDTO responseDTO = await paymentsService.GetPaymentByTransactionId(transactionId);

            if (responseDTO is null) return NotFound($"Transcation Id {transactionId} not found");

            return Ok(responseDTO);
        }

        [Authorize]
        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<PaymentsResponseDTO>> GetPaymentsByOrderId(long orderId)
        {
            PaymentsResponseDTO? responseDTO = await paymentsService.GetPaymentsByOrderIdAsync(orderId);

            if (responseDTO is null) return NotFound($"Payment for Order Id {orderId} not found");

            return Ok(responseDTO);
        }

        [Authorize(Policy = "CourierOnly")]
        [HttpPost("propose")]
        public async Task<ActionResult<PaymentsResponseDTO>> ProposeItemsFeeAsync([FromForm] ProposePaymentRequestDTO proposePaymentRequestDTO)
        {
            PaymentsResponseDTO? response = await paymentsService.ProposeItemsFeeAsync(proposePaymentRequestDTO);

            if (response is null) return NotFound($"Order Id {proposePaymentRequestDTO.OrderIdFK} is not found");

            await orderHub.Clients.Group($"ORDER_{proposePaymentRequestDTO.OrderIdFK}").SendAsync("PaymentProposal", response);

            return Ok(response);
        }

        [Authorize(Policy = "CustomerOnly")]
        [HttpPatch("propose/accept/{orderId}")]
        public async Task<ActionResult<PaymentsResponseDTO>> AcceptProposedItemsFeeAsync(long orderId)
        {
            PaymentsResponseDTO? responseDTO = await paymentsService.AcceptProposedItemsFeeAsync(orderId);

            if (responseDTO is null) return NotFound($"Order Id {orderId} is not found");

            await orderHub.Clients.Group($"ORDER_{orderId}").SendAsync("PaymentProposal", responseDTO);

            return Ok(responseDTO);
        }
    }
}