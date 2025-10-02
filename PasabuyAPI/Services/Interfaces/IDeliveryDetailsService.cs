using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Models;

namespace PasabuyAPI.Services.Interfaces
{
    public interface IDeliveryDetailsService
    {
        Task<DeliveryDetailsResponseDTO> CreateDeliveryDetails(DeliveryDetailsRequestDTO deliveryDetailsRequestDTO);
    }
}