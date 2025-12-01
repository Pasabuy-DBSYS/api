using PasabuyAPI.Enums;
using PasabuyAPI.Models;

namespace PasabuyAPI.Repositories.Interfaces
{
    public interface IPaymentsRepository
    {
        Task<Payments> CreatePayment(Priority Urgency, decimal Distance, Payments payment);
        Task<Payments> GetPaymentsByTransactionIdAsync(string TransactionId);
        Task<Payments> GetPaymentsByOrderIdAsync(long OrderId);
        Task<Payments> AcceptProposedItemsFeeAsync(long orderId);
        Task<Payments> RejectProposedItemsFeeAsync(long orderId);
        Task<Payments> ProposeItemsFeeAsync(long orderId, decimal proposedItemsFee, string imagePath);
        Task<Payments> UpdatePaymentStatusAsync(long orderId, PaymentStatus paymentStatus);
    }
}