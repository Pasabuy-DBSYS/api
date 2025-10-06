using Mapster;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Services.Implementations
{
    public class PaymentsService(IPaymentsRepository paymentsRepository) : IPaymentsService
    {

        public async Task<PaymentsResponseDTO> GetPaymentByTransactionId(string transactionId)
        {
            Payments payment = await paymentsRepository.GetPaymentsByTransactionIdAsync(transactionId);

            return payment.Adapt<PaymentsResponseDTO>();
        }

        public async Task<PaymentsResponseDTO?> ProposeItemsFeeAsync(long orderId, decimal proposedItemsFee)
        {
            Payments? entity = await paymentsRepository.ProposeItemsFeeAsync(orderId, proposedItemsFee);

            if (entity is null) return null;

            return entity.Adapt<PaymentsResponseDTO>();
        }

        public async Task<PaymentsResponseDTO?> AcceptProposedItemsFeeAsync(long orderId)
        {
            Payments? entity = await paymentsRepository.AcceptProposedItemsFeeAsync(orderId);

            if (entity is null) return null;

            return entity.Adapt<PaymentsResponseDTO>();
        }
    }
}