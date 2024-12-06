using AutoMapper;
using ChatService.BLL.Models;
using ChatService.BLL.Services.IServices;
using ChatService.BLL.Utilities.Exceptions;
using ChatService.BLL.Utilities.Messages;
using ChatService.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController(IChatService chatService, IMapper mapper) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ChatViewModel> GetChat(Guid id, CancellationToken cancellationToken)
        {
            var chat = await chatService.GetChatAsync(id, GetAuth0IdFromContext(), cancellationToken);

            return mapper.Map<ChatViewModel>(chat);
        }

        [HttpGet("my-estate")]
        public async Task<IEnumerable<ChatViewModel>> GetEstateChatList(CancellationToken cancellationToken)
        {
            var userId = GetAuth0IdFromContext();

            var chatList = await chatService.GetEstateChatListAsync(userId, cancellationToken);

            return mapper.Map<IEnumerable<ChatModel>, IEnumerable<ChatViewModel>>(chatList);
        }

        [HttpGet("my")]
        public async Task<IEnumerable<ChatViewModel>> GetUserChatList(CancellationToken cancellationToken)
        {
            var userId = GetAuth0IdFromContext();

            var chatList = await chatService.GetUserChatListAsync(userId, cancellationToken);

            return mapper.Map<IEnumerable<ChatModel>, IEnumerable<ChatViewModel>>(chatList);
        }

        [HttpPost("estate/{estateId}")]
        public async Task<ChatViewModel> CreateChat(Guid estateId, CancellationToken cancellationToken)
        {
            var id = GetAuth0IdFromContext();

            var createdChat = await chatService.CreateChatAsync(id, estateId, cancellationToken);

            return mapper.Map<ChatViewModel>(createdChat);
        }

        private string GetAuth0IdFromContext()
        {
            return HttpContext.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
    }
}
