using PasabuyAPI.Enums;
using PasabuyAPI.Models;

namespace PasabuyAPI.Repositories.Interfaces
{
    public interface IPaymentsRepository
    {
        Task<Payments> CreatePayment(Priority Urgency, decimal Distance, Payments payment);
        Task<Payments> GetPaymentsByTransactionIdAsync(string TransactionId);
        Task<Payments?> AcceptProposedItemsFeeAsync(long orderId);
        Task<Payments?> ProposeItemsFeeAsync(long orderId, decimal proposedItemsFee);
    }
}