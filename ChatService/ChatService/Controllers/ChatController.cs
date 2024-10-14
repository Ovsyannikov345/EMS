using ChatService.BLL.Models;
using ChatService.BLL.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController(IChatService chatService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ChatModel> Test(Guid id, CancellationToken cancellationToken)
        {
            return await chatService.GetChatAsync(id, cancellationToken);
        }
    }
}
