using Mapster;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Services.Implementations
{
    public class ReviewsService(IReviewsRepository reviewsRepository) : IReviewsService
    {
        private readonly IReviewsRepository _reviewsRepository = reviewsRepository;

        public async Task<ReviewResponseDTO> CreateReviewAsync(CreateReviewRequestDTO reviewDto)
        {
            bool existsByOrder = await _reviewsRepository.ExistsByOrderIdAsync(reviewDto.OrderIDFK);
            if (existsByOrder)
                throw new InvalidOperationException("A review for this order already exists.");

            Reviews reviewEntity = reviewDto.Adapt<Reviews>();

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
