using ChatService.BLL.Models;

namespace ChatService.BLL.Services.IServices
{
    public interface IMessageService
    {
        public Task<MessageModel> CreateAsync(MessageModel messageData, CancellationToken cancellationToken = default);
    }
}
