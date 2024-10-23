using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApiServer.Data;
using WebApiServer.Models;
using WebApiServer.Hubs;

namespace WebApiServer.Controllers
{
    [ApiController]
    [Route("api/message")]
    public class MessageController : ControllerBase
    {
        private readonly MessageRepository messageRepository;
        private readonly IHubContext<MessageHub> hubContext;
        private readonly ILogger logger;

        public MessageController(MessageRepository _messageRepository, IHubContext<MessageHub> _hubContext, ILogger<MessageController> _logger)
        {
            messageRepository = _messageRepository;
            hubContext = _hubContext;
            logger = _logger;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] Message message)
        {
            logger.LogInformation($"Received message {message.Number}: {message.Content} | {message.Timestamp}");

            try
            {
                message.Timestamp = DateTime.UtcNow;
                await messageRepository.AddMessageAsync(message);
                logger.LogInformation("Message saved to DataBase");

                await hubContext.Clients.All.SendAsync("ReceiveMessage", message.Content, message.Timestamp, message.Number);
                logger.LogInformation("Message sent via SignalR");

                return Ok(message);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error while saving to DB: {ex.Message}");
                return StatusCode(500, new Message());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesMinuteAgo()
        {
            try
            {
                var timeMinuteAgo = DateTime.UtcNow.AddMinutes(-1);
                var recentMessages = await messageRepository.GetMessagesMinuteAgoAsync(timeMinuteAgo);
                logger.LogInformation($"Got {recentMessages.Count} recent messages");

                return Ok(recentMessages);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error getting recent messages: {ex.Message}");
                return StatusCode(500, new List<Message>());
            }
        }
    }
}
