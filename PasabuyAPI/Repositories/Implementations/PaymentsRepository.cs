using PasabuyAPI.Data;
using PasabuyAPI.Enums;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;

namespace PasabuyAPI.Repositories.Implementations
{
    public class PaymentsRepository(PasabuyDbContext pasabuyDbContext) : IPaymentsRepository
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

            payment.BaseFee = BASE_FEE;

            await pasabuyDbContext.AddAsync(payment);
            await pasabuyDbContext.SaveChangesAsync();
            return payment;
        }
    }
}