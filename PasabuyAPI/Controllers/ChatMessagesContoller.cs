using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PasabuyAPI.DTOs.Requests;
using PasabuyAPI.DTOs.Responses;
using PasabuyAPI.Hubs;
using PasabuyAPI.Services.Implementations;
using PasabuyAPI.Services.Interfaces;

namespace PasabuyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatMessagesController(IChatMessagesService chatMessagesService, IHubContext<ChatHub> chatHub) : ControllerBase
    {
        private readonly IChatMessagesService _chatMessagesService = chatMessagesService;
        private readonly IHubContext<ChatHub> _chatHub = chatHub;

        [Authorize(Policy = "VerifiedOnly")]
        [HttpPost("send")]
        public async Task<ActionResult<ChatMessagesResponseDTO>> SendMessageAsync([FromBody] SendMessageRequestDTO sendMessageRequestDTO)
        {
            var savedMessage = await _chatMessagesService.SendMessage(sendMessageRequestDTO);

            await _chatHub.Clients
                    .Group(sendMessageRequestDTO.RoomIdFK.ToString())
                    .SendAsync("ReceiveMessage", savedMessage);

            return Ok(savedMessage);
        }

        [Authorize(Policy = "VerifiedOnly")]
        [HttpGet("{roomId}")]
        public async Task<ActionResult<ChatMessagesResponseDTO>> GetAllMesagesByRoomId(long roomId)
        {
            var userData = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!long.TryParse(userData, out var courierId))
                return BadRequest("Invalid courier ID in token.");            

            var savedMessage = await _chatMessagesService.GetChatMessagesByRoomIdAsync(roomId, courierId);

            return Ok(savedMessage);
        }
    }
}