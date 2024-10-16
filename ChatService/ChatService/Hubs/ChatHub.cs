using AutoMapper;
using ChatService.BLL.Models;
using ChatService.BLL.Services.IServices;
using ChatService.BLL.Utilities.Exceptions;
using ChatService.BLL.Utilities.Messages;
using ChatService.DAL.Grpc.Services.IServices;
using ChatService.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatService.BLL.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub(IChatService chatService, IMessageService messageService, IProfileGrpcClient profileGrpcClient, IMapper mapper) : Hub
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

        public async Task Send(Guid chatId, string message)
        {
            string auth0Id = GetAuth0IdFromContext();

            var chat = await chatService.GetChatAsync(chatId);

            if (auth0Id == chat.Estate.User.Auth0Id)
            {
                _connectedUsers.TryGetValue(chat.User.Auth0Id, out List<string>? userConnections);

                var profile = await GetUserProfile(auth0Id);

                await CreateAndSendMessageAsync(new MessageModel() { Text = message, ChatId = chatId, UserId = profile.Id }, userConnections);
            }
            else if (auth0Id == chat.User.Auth0Id)
            {
                _connectedUsers.TryGetValue(chat.Estate.User.Auth0Id, out List<string>? estateOwnerConnections);

                var profile = await GetUserProfile(auth0Id);

                await CreateAndSendMessageAsync(new MessageModel() { Text = message, ChatId = chatId, UserId = profile.Id }, estateOwnerConnections);
            }
        }

        private async Task CreateAndSendMessageAsync(MessageModel messageModel, IEnumerable<string>? connectionIds = default, CancellationToken cancellationToken = default)
        {
            var createdMessage = await messageService.CreateAsync(messageModel, cancellationToken);

            if (connectionIds == null)
            {
                return;
            }

            foreach (var connectionId in connectionIds)
            {
                await Clients.Client(connectionId).SendAsync("Receive", mapper.Map<MessageViewModel>(createdMessage), cancellationToken);
            }
        }

        private string GetAuth0IdFromContext()
        {
            return Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }

        private async Task<UserProfileModel> GetUserProfile(string auth0Id)
        {
            var profileResponse = await profileGrpcClient.GetOwnProfile(auth0Id);

            return mapper.Map<UserProfileModel>(profileResponse.Profile ?? throw new NotFoundException(ProfileMessages.ProfileNotFound));
        }
    }
}
