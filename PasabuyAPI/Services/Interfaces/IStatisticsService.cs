using PasabuyAPI.DTOs.Responses;

namespace PasabuyAPI.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<CustomerStatisticsResponseDTO> GetCustomerStatistics(long customerId);
        Task<CourierStatisticsResponseDTO> GetCourierStatistics(long courierId);
    }
}