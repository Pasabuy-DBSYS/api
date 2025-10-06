using Microsoft.EntityFrameworkCore;
using PasabuyAPI.Data;
using PasabuyAPI.Enums;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;

namespace PasabuyAPI.Repositories.Implementations
{
    public class PaymentsRepository(PasabuyDbContext _context) : IPaymentsRepository
    {
        private readonly decimal BASE_FEE = 10.0m;
        private readonly decimal URGENCY_FEE = 5.0m;
        private readonly decimal FEE_PER_KM = 5.0m;
        public async Task<Payments> CreatePayment(Priority Urgency, decimal Distance, Payments payment)
        {
            payment.DeliveryFee = BASE_FEE + (FEE_PER_KM * Distance);

            if (Urgency == Priority.URGENT)
            {
                payment.DeliveryFee += URGENCY_FEE;
                payment.UrgencyFee = 5.0m;
            }
            else
            {
                payment.UrgencyFee = 0m;
            }

            payment.BaseFee = BASE_FEE;

            await _context.AddAsync(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payments?> GetPaymentsByTransactionIdAsync(string TransactionId)
        {
            if (!Guid.TryParse(TransactionId, out var transactionGuid))
                return null; // invalid transaction id format

            return await _context.Payments
                .AsNoTracking()
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.TransactionId == transactionGuid);
        }
    }
}