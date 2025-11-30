using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Exceptions;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Services.Implementations
{
    public class ReviewsService(IReviewsRepository reviewsRepository, IOrderService orderService) : IReviewsService
    {
        private readonly IReviewsRepository _reviewsRepository = reviewsRepository;

        public async Task<ReviewResponseDTO> CreateReviewAsync(CreateReviewRequestDTO reviewDto, long reviewerId)
        {
            var order = await orderService.GetOrderByOrderId(reviewDto.OrderIDFK)?? throw new NotFoundException("Order not found");

            if(reviewerId != order.CustomerId || reviewerId != order.CourierId)
            {
                throw new UnauthorizedAccessException("You can't review the order");
            }
            
            Reviews reviewEntity = reviewDto.Adapt<Reviews>();

            reviewEntity.ReviewerIDFK = reviewerId;
            reviewEntity.CreatedAt = DateTime.UtcNow;

            var createdReview = await _reviewsRepository.CreateReview(reviewEntity);

            return createdReview.Adapt<ReviewResponseDTO>();
        }

        public async Task<List<ReviewResponseDTO>> GetAllReviewsAsync()
        {
            var reviews = await _reviewsRepository.GetAllReviews();
            return reviews.Adapt<List<ReviewResponseDTO>>();
        }

        public async Task<ReviewResponseDTO?> GetReviewByIdAsync(long id)
        {
            var review = await Task.Run(() => _reviewsRepository.GetReviewsById(id)); 
            if (review == null)
                return null;

            return review.Adapt<ReviewResponseDTO>();
        }
    }
}
