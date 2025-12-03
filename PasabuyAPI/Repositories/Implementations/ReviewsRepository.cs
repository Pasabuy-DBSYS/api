using Microsoft.EntityFrameworkCore;
using PasabuyAPI.Data;
using PasabuyAPI.Exceptions;
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
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserIdPK == review.ReviewedUserID) ?? throw new NotFoundException($"User not found {review.ReviewedUserID}");
            var rating = await GetAverageRatingByReviewedIdAsync(user.UserIdPK);
            user.RatingAverage = rating;
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

        public async Task<decimal> GetAverageRatingByReviewedIdAsync(long reviewedId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.ReviewedUserID == reviewedId)
                .ToListAsync();
            
            if (reviews.Count == 0)
                return 0;

            return Math.Round((decimal)reviews.Average(r => r.Rating), 1);
        }
    }
}
