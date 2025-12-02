using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Services.Implementations
{
    public class StatisticsService(IPaymentsRepository paymentsRepository, IReviewsRepository reviewsRepository, IOrderRepository orderRepository) : IStatisticsService
    {
        public async Task<CourierStatisticsResponseDTO> GetCourierStatistics(long courierId)
        {
            CourierStatisticsResponseDTO courierStatisticsResponseDTO = new()
            {
                TotalEarnings = await paymentsRepository.GetCourierTotalEarnings(courierId),
                TotalDeliveries = await orderRepository.GetTotalDeliveries(courierId),
                UserId = courierId,
                Rating = await reviewsRepository.GetAverageRatingByReviewedIdAsync(courierId)
            };

            return courierStatisticsResponseDTO;
        }

        public async Task<CustomerStatisticsResponseDTO> GetCustomerStatistics(long customerId)
        {
            CustomerStatisticsResponseDTO customerStatisticsResponseDTO = new()
            {
                TotalSpent = await paymentsRepository.GetCustomerTotalSpending(customerId),
                TotalOrders = await orderRepository.GetTotalOrders(customerId),
                UserId = customerId,
                TotalRating = await reviewsRepository.GetAverageRatingByReviewedIdAsync(customerId)
            };

            return customerStatisticsResponseDTO;
        }
    }
}