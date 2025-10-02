using PasabuyAPI.Data;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;

namespace PasabuyAPI.Repositories.Implementations
{
    public class DeliveryDetailsRepository(PasabuyDbContext context) : IDeliveryDetailsRepository
    {
        public async Task<DeliveryDetails> CreateDeliveryDetails(DeliveryDetails deliveryDetails)
        {
            await context.AddAsync(deliveryDetails);
            await context.SaveChangesAsync();
            return deliveryDetails;
        }
    }
}