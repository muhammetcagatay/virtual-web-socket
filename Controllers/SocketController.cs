using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using virtual_web_socket.Dtos;
using virtual_web_socket.Handlers;

namespace virtual_web_socket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocketController : ControllerBase
    {
        private readonly WebSocketHandler _webSocketHandler;

        public SocketController(WebSocketHandler webSocketHandler)
        {
            _webSocketHandler = webSocketHandler;
        }

        [HttpPost("sendMessage")]
        public async Task<IActionResult> SendMessage([FromQuery] SocketSendMessageRequestDto request, [FromBody] object message)
        {
            var result = await _webSocketHandler.SendMessageAsync(request, JsonSerializer.Serialize(message));

            return Ok(result);
        }
    }
}
