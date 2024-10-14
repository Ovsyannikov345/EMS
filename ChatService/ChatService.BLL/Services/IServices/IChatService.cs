using ChatService.BLL.Models;

namespace ChatService.BLL.Services.IServices
{
    public interface IChatService
    {
        public Task<ChatModel> GetChatAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
