using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;

namespace PasabuyAPI.Services.Interfaces
{
    public interface IPaymentsService
    {
        Task<PaymentsResponseDTO> GetPaymentByTransactionId(string transactionId);
        Task<PaymentsResponseDTO?> AcceptProposedItemsFeeAsync(long orderId);
        Task<PaymentsResponseDTO?> ProposeItemsFeeAsync(long orderId, decimal proposedItemsFee);
    }
}