using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatService.BLL.Hubs
{
    public class ChatHub : Hub
    {
        public static Dictionary<string, List<string>> ConnectedUsers = new();

        private static readonly object _lock = new();

        public override Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;

            var userId = GetAuth0IdFromContext();

            lock (_lock)
            {
                if (!ConnectedUsers.TryGetValue(userId, out List<string>? connections))
                {
                    connections = new();
                    ConnectedUsers[userId] = connections;
                }

                connections.Add(connectionId);
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;

            var userId = GetAuth0IdFromContext();

            lock (_lock)
            {
                if (ConnectedUsers.TryGetValue(userId, out List<string>? connections))
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        ConnectedUsers.Remove(userId);
                    }
                }
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendAsync(Guid chatId, string message)
        {
            string auth0Id = GetAuth0IdFromContext();

            

            // get chat

            // 2 scenarios: user / estate owner

            // check if estate user / owner has online connections

            // send message to all connections
        }

        private string GetAuth0IdFromContext()
        {
            return Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
    }
}
