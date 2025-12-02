using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatRoomController(IChatRoomService chatRoomService) : ControllerBase
    {
        [Authorize]
        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<ChatRoomResponseDTO>> GetChatRoomByOrderId(long orderId)
        {
            var chatRoom = await chatRoomService.GetChatRoomByOrderId(orderId);
            return Ok(chatRoom);
        }

        [Authorize]
        [HttpPatch("close/{roomId}")]
        public async Task<ActionResult<bool>> CloseChatRoom(long roomId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized("Invalid token — user ID not found.");

            if (!long.TryParse(userIdClaim, out var userId))
                return BadRequest("Invalid user ID format.");

            var result = await chatRoomService.CloseChatRoomAsync(roomId, userId);
            return Ok(result);
        }
    }
}
