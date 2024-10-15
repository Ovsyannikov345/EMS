using AutoMapper;
using ChatService.BLL.Models;
using ChatService.BLL.Services.IServices;
using ChatService.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatService.BLL.Hubs
{
    [Authorize]
    public class ChatHub(IChatService chatService, IMessageService messageService, IMapper mapper) : Hub
    {
        private static Dictionary<string, List<string>> _connectedUsers = new();

        private static readonly object _lock = new();

        public override Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;

            var userId = GetAuth0IdFromContext();

            lock (_lock)
            {
                if (!_connectedUsers.TryGetValue(userId, out List<string>? connections))
                {
                    connections = new();
                    _connectedUsers[userId] = connections;
                }

                connections.Add(connectionId);
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = Context.ConnectionId;

            var userId = GetAuth0IdFromContext();

            lock (_lock)
            {
                if (_connectedUsers.TryGetValue(userId, out List<string>? connections))
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        _connectedUsers.Remove(userId);
                    }
                }
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendAsync(Guid chatId, string message, CancellationToken cancellationToken = default)
        {
            string auth0Id = GetAuth0IdFromContext();

            var chat = await chatService.GetChatAsync(chatId, cancellationToken);

            if (auth0Id == chat.Estate.User.Auth0Id)
            {
                var userConnections = _connectedUsers[chat.User.Auth0Id];

                await CreateAndSendMessageAsync(new MessageModel() { Text = message, ChatId = chatId }, userConnections, cancellationToken);
            }
            else if (auth0Id == chat.User.Auth0Id)
            {
                var estateOwnerConnections = _connectedUsers[chat.Estate.User.Auth0Id];

                await CreateAndSendMessageAsync(new MessageModel() { Text = message, ChatId = chatId }, estateOwnerConnections, cancellationToken);
            }
        }

        private async Task CreateAndSendMessageAsync(MessageModel messageModel, IEnumerable<string> connectionIds, CancellationToken cancellationToken = default)
        {
            var createdMessage = await messageService.CreateAsync(messageModel, cancellationToken);

            foreach (var connectionId in connectionIds)
            {
                await Clients.Client(connectionId).SendAsync("Receive", mapper.Map<MessageViewModel>(createdMessage), cancellationToken);
            }
        }

        private string GetAuth0IdFromContext()
        {
            return Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
    }
}
