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

        [HttpPost("send")]
        public async Task<ActionResult<ChatMessagesResponseDTO>> SendMessageAsync([FromBody] SendMessageRequestDTO sendMessageRequestDTO)
        {
            var savedMessage = await _chatMessagesService.SendMessage(sendMessageRequestDTO);

            await _chatHub.Clients
                    .Group(sendMessageRequestDTO.RoomIdFK.ToString())
                    .SendAsync("ReceiveMessage", savedMessage);

            return Ok(savedMessage);
        }
    }
}