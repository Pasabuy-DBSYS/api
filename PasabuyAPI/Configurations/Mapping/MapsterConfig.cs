using Mapster;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Models;

namespace PasabuyAPI.Configurations.Mapping
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<Users, UserResponseDTO>.NewConfig()
                .Map(dest => dest.UserIdPK, src => src.UserIdPK)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.Username, src => src.Username)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.MiddleName, src => src.MiddleName)
                .Map(dest => dest.LastName, src => src.LastName)
                .Map(dest => dest.Phone, src => src.Phone)
                .Map(dest => dest.Birthday, src => src.Birthday)
                .Map(dest => dest.RatingAverage, src => src.RatingAverage)
                .Map(dest => dest.TotalDeliveries, src => src.TotalDeliveries)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt)
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
                .Map(dest => dest.IsActive, src => src.IsActive)
                .Map(dest => dest.VerifiactionInfoDTO, src => src.VerificationInfo);
            // .IgnoreNullValues(true);

            TypeAdapterConfig<UserRequestDTO, Users>.NewConfig()
                .Ignore(dest => dest.UserIdPK)
                .Ignore(dest => dest.CustomerOrders)
                .Ignore(dest => dest.CourierOrders)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.UpdatedAt)
                .Ignore(dest => dest.VerificationInfo)
                .Map(dest => dest.PasswordHash, src => src.Password); // if not hashing yet

            TypeAdapterConfig<Orders, OrderResponseDTO>.NewConfig()
                .Map(dest => dest.OrderIdPK, src => src.OrderIdPK)
                .Map(dest => dest.Request, src => src.Request)
                .Map(dest => dest.Status, src => src.Status.ToString())
                .Map(dest => dest.Priority, src => src.Priority.ToString())
                .Map(dest => dest.CustomerId, src => src.CustomerId)
                .Map(dest => dest.CourierId, src => src.CourierId)
                .Map(dest => dest.Created_at, src => src.Created_at)
                .Map(dest => dest.Updated_at, src => src.Updated_at)
                .Map(dest => dest.DeliveryDetailsDTO, src => src.DeliveryDetails)
                .Map(dest => dest.PaymentsResponseDTO, src => src.Payment);
        }
    }
}