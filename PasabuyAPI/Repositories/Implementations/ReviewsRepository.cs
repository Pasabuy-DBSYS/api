using Microsoft.EntityFrameworkCore;
using PasabuyAPI.Data;
using PasabuyAPI.Models;
using PasabuyAPI.Repositories.Interfaces;

namespace PasabuyAPI.Repositories.Implementations
{
    public class ReviewsRepository(PasabuyDbContext context) : IReviewsRepository
    {
        private readonly PasabuyDbContext _context = context;

        public async Task<Reviews> CreateReview(Reviews review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<List<Reviews>> GetAllReviews()
        {
            return await _context.Reviews.ToListAsync();
        }


        public Reviews? GetReviewsById(long id)
        {
            return _context.Reviews.FirstOrDefault(r => r.ReviewIDPK == id);
        }

        // Helpers
        public async Task<bool> ExistsByIdAsync(long id)
        {
            return await _context.Reviews.AnyAsync(r => r.ReviewIDPK == id);
        }

        public async Task<bool> ExistsByOrderIdAsync(long orderId)
        {
            return await _context.Reviews.AnyAsync(r => r.OrderIDFK == orderId);
        }

        public async Task<bool> ExistsByReviewerIdAsync(long reviewerId)
        {
            return await _context.Reviews.AnyAsync(r => r.ReviewedUserID == reviewerId);
        }
    }
}
