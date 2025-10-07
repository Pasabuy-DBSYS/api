using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Enums;
using PasabuyAPI.Models;

namespace PasabuyAPI.Services.Interfaces
{
    public interface IPaymentsService
    {
        Task<PaymentsResponseDTO> GetPaymentByTransactionId(string transactionId);
        Task<PaymentsResponseDTO?> GetPaymentsByOrderIdAsync(long OrderId);
        Task<PaymentsResponseDTO?> AcceptProposedItemsFeeAsync(long orderId);
        Task<PaymentsResponseDTO?> ProposeItemsFeeAsync(long orderId, decimal proposedItemsFee);
        Task<PaymentsResponseDTO> UpdatePaymentStatusAsync(long orderId, PaymentStatus paymentsStatus);
    }
}