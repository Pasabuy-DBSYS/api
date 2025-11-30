using PasabuyAPI.Models;

namespace PasabuyAPI.Repositories.Interfaces
{
    public interface IReviewsRepository
    {
        Task<Reviews> CreateReview(Reviews review);
        Task<List<Reviews>> GetAllReviews();
        Reviews? GetReviewsById(long id);

        //helpers

        Task<bool> ExistsByIdAsync(long id);
        // Task<bool> ExistsByOrderIdAsync(long orderId, long reviewerId);
        Task<bool> ExistsByReviewerIdAsync(long reviewerId);

    }
}