using Mapster;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Implementations;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Services.Implementations
{
    public class DeliveryDetailsService(IOrderRepository orderRepository, IDeliveryDetailsRepository deliveryDetailsRepository) : IDeliveryDetailsService
    {
        public async Task<DeliveryDetailsResponseDTO> CreateDeliveryDetails(DeliveryDetailsRequestDTO deliveryDetailsRequestDTO)
        {
            if (deliveryDetailsRequestDTO.OrderIdPK is not null)
            {
                var order = await orderRepository.GetOrderByOrderId(deliveryDetailsRequestDTO.OrderIdPK.Value) ??
                throw new Exception($"Order with ID {deliveryDetailsRequestDTO.OrderIdPK.Value} not found.");
            }

            DeliveryDetails entity = deliveryDetailsRequestDTO.Adapt<DeliveryDetails>();
            DeliveryDetails savedEntity = await deliveryDetailsRepository.CreateDeliveryDetails(entity);
            return savedEntity.Adapt<DeliveryDetailsResponseDTO>();
        }
    }
}