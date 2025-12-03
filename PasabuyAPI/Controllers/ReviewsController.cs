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
using Microsoft.AspNetCore.SignalR;
using PasabuyAPI.Hubs;
using PasabuyAPI.Repositories.Interfaces;
using PasabuyAPI.Exceptions;

namespace PasabuyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController(IReviewsService reviewsService, IOrderService orderService, INotificationService notificationService, IUserService userService ,IHubContext<NotificationHub> notificationsHub) : ControllerBase
    {
        private readonly IReviewsService _reviewsService = reviewsService;
        private readonly IOrderService _orderService = orderService;

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

                // Update order review flags based on who reviewed
                var order = await _orderService.GetOrderByOrderId(reviewData.OrderIDFK);
                if (order != null)
                {
                    // Customer review branch
                    if (order.CustomerId == userId && !order.IsCustomerReviewed)
                    {
                        await _orderService.UpdateCustomerReviewedStatus(userId, true, reviewData.OrderIDFK);

                        var customerThankYouDto = new NotificationRequestDTO
                        {
                            Title = "Order Reviewed",
                            Message = $"Thank you for your review [Order #{order.OrderIdPK}]",
                            Pressed = false,
                            UserIdFk = userId
                        };
                        var customerThankYouResp = await notificationService.CreateNotification(customerThankYouDto);
                        await notificationsHub.Clients.Group($"user:{userId}").SendAsync("ReceiveNotification", customerThankYouResp);

                        if (order.CourierId != 0)
                        {
                            var courierInformedDto = new NotificationRequestDTO
                            {
                                Title = "Order Reviewed",
                                Message = $"[Order #{order.OrderIdPK}] has been reviewed by the customer",
                                Pressed = false,
                                UserIdFk = order.CourierId
                            };
                            var courierInformedResp = await notificationService.CreateNotification(courierInformedDto);
                            await notificationsHub.Clients.Group($"user:{order.CourierId}").SendAsync("ReceiveNotification", courierInformedResp);
                        }
                    }
                    // Courier review branch
                    else if (order.CourierId != 0 && order.CourierId == userId && !order.IsCourierReviewed)
                    {
                        await _orderService.UpdateCourierReviewedStatus(userId, true, reviewData.OrderIDFK);

                        var courierThankYouDto = new NotificationRequestDTO
                        {
                            Title = "Order Reviewed",
                            Message = $"Thank you for your review [Order #{order.OrderIdPK}]",
                            Pressed = false,
                            UserIdFk = userId
                        };
                        var courierThankYouResp = await notificationService.CreateNotification(courierThankYouDto);
                        await notificationsHub.Clients.Group($"user:{userId}").SendAsync("ReceiveNotification", courierThankYouResp);

                        // Inform customer that courier reviewed
                        var customerInformedDto = new NotificationRequestDTO
                        {
                            Title = "Order Reviewed",
                            Message = $"[Order #{order.OrderIdPK}] has been reviewed by the courier",
                            Pressed = false,
                            UserIdFk = order.CustomerId
                        };
                        var customerInformedResp = await notificationService.CreateNotification(customerInformedDto);
                        await notificationsHub.Clients.Group($"user:{order.CustomerId}").SendAsync("ReceiveNotification", customerInformedResp);
                    }
                }

                var user = await userService.GetUserByIdAsync(reviewData.ReviewedUserID) ?? throw new NotFoundException($"User Id {reviewData.ReviewedUserID} not found");
                await notificationsHub.Clients.Group($"user:{reviewData.ReviewedUserID}").SendAsync("NewReviewReceived", user.RatingAverage);

                return CreatedAtAction(
                    actionName: nameof(GetReviewAsync),
                    routeValues: new { id = review.ReviewIDPK },
                    value: review);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message});
            }
        }
        
    }
}