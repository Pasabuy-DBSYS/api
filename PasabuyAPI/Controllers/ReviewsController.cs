using Microsoft.AspNetCore.Mvc;
using PasabuyAPI.Models;
using PasabuyAPI.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using PasabuyAPI.Services.Interfaces;
using System.Threading.Tasks;
using Mapster;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace PasabuyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController(IReviewsService reviewsService) : ControllerBase
    {
        private readonly IReviewsService _reviewsService = reviewsService;

        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<ActionResult<List<ReviewResponseDTO>>> GetReviewsAsync()
        {
            return Ok(await _reviewsService.GetAllReviewsAsync());
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewResponseDTO>> GetReviewAsync(long id)
        {
            ReviewResponseDTO? review = await _reviewsService.GetReviewByIdAsync(id);
            if (review == null) return NotFound();

            return Ok(review);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ReviewResponseDTO>> CreateReviewAsync([FromBody] CreateReviewRequestDTO reviewData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userIdClaim == null)
                    return Forbid("Invalid token");

                if (!long.TryParse(userIdClaim, out var userId))
                    return Forbid("Invalid Id");

                ReviewResponseDTO review = await _reviewsService.CreateReviewAsync(reviewData, userId);
                Console.WriteLine($"Created ReviewIDPK: {review.ReviewIDPK}");
                return CreatedAtAction(
                    actionName: nameof(GetReviewAsync),
                    routeValues: new { id = review.ReviewIDPK },
                    value: review);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        
    }
}