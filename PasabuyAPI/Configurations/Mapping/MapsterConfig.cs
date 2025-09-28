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
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Phone, src => src.Phone)
                .Map(dest => dest.Birthday, src => src.Birthday)
                .Map(dest => dest.YearLevel, src => src.YearLevel)
                .Map(dest => dest.RatingAverage, src => src.RatingAverage)
                .Map(dest => dest.TotalDeliveries, src => src.TotalDeliveries)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt)
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
                .Map(dest => dest.IsActive, src => src.IsActive)
                .Map(dest => dest.CustomerOrders, src => src.CustomerOrders)
                .Map(dest => dest.CourierOrders, src => src.CourierOrders)
                .IgnoreNullValues(true);

            TypeAdapterConfig<UserRequestDTO, Users>.NewConfig()
                .Ignore(dest => dest.UserIdPK)
                .Ignore(dest => dest.CustomerOrders)
                .Ignore(dest => dest.CourierOrders)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.UpdatedAt)
                .Map(dest => dest.PasswordHash, src => src.Password); // if not hashing yet

            TypeAdapterConfig<Orders, OrderResponseDTO>.NewConfig()
                .Map(dest => dest.OrderIdPK, src => src.OrderIdPK)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Status, src => src.Status.ToString())
                .Map(dest => dest.Priority, src => src.Priority.ToString())
                .Map(dest => dest.CustomerId, src => src.CustomerId)
                .Map(dest => dest.CourierId, src => src.CourierId)
                .Map(dest => dest.CustomerName, src => src.Customer.Name) // Map customer name
                .Map(dest => dest.CourierName, src => src.Courier != null ? src.Courier.Name : null) // Handle nullable Courier
                .Map(dest => dest.Created_at, src => src.Created_at)
                .Map(dest => dest.Updated_at, src => src.Updated_at);
        }
    }
}