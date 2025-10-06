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
        private readonly decimal BASE_FEE = 10.0m;
        private readonly decimal URGENCY_FEE = 5.0m;
        private readonly decimal FEE_PER_KILOMETER = 5.0m;

        public async Task<PaymentsResponseDTO> GetPaymentByTransactionId(string transactionId)
        {
            Payments payment = await paymentsRepository.GetPaymentsByTransactionIdAsync(transactionId);

            return payment.Adapt<PaymentsResponseDTO>();
        }
    }
}