using PasabuyAPI.Models;

namespace PasabuyAPI.Repositories.Interfaces
{
    public interface IReviewsRepository
    {
        Task<Reviews> CreateReview(Reviews review);
        Task<List<Reviews>> GetAllReviews();
        Reviews? GetReviewsById(long id);
        Task<decimal> GetAverageRatingByReviewedIdAsync(long reviewedId);

    }
}