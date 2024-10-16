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
            var chat = await chatService.GetChatAsync(id, cancellationToken);

            var userId = GetAuth0IdFromContext();

            if (userId != chat.User.Auth0Id && userId != chat.Estate.User.Auth0Id)
            {
                throw new ForbiddenException(ChatMessages.ChatAccessDenied);
            }

            return mapper.Map<ChatViewModel>(chat);
        }

        private string GetAuth0IdFromContext()
        {
            return HttpContext.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
    }
}
