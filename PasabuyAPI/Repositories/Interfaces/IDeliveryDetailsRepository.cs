using PasabuyAPI.Models;

namespace PasabuyAPI.Repositories.Interfaces
{
    public interface IDeliveryDetailsRepository
    {
        Task<DeliveryDetails> CreateDeliveryDetails(DeliveryDetails deliveryDetails);
    }
}