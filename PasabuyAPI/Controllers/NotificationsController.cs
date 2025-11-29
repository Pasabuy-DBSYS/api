using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController(INotificationService notificationService) : ControllerBase
    {
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<ActionResult<List<NotificationResponseDTO>>> GetAllNotifications()
        {
            var notifications = await notificationService.GetNotifications();
            return Ok(notifications);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationResponseDTO>> GetNotificationById(long id)
        {
            var notification = await notificationService.GetNotificationByid(id);

            if (notification == null)
                return NotFound($"Notification with id {id} not found.");

            return Ok(notification);
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<ActionResult<List<NotificationResponseDTO>>> GetUserNotifications()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized("Invalid token — user ID not found.");

            if (!long.TryParse(userIdClaim, out var userId))
                return BadRequest("Invalid user ID format.");

            var notifications = await notificationService.GetNotificayionsByUserId(userId);
            return Ok(notifications);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<NotificationResponseDTO>> CreateNotification([FromBody] NotificationRequestDTO notificationRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var notification = await notificationService.CreateNotification(notificationRequest);
            return StatusCode(201, notification);
        }
    }
}
