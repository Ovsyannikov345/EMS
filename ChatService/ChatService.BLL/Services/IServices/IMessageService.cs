using ChatService.BLL.Models;

namespace ChatService.BLL.Services.IServices
{
    public interface IMessageService
    {
        Task<MessageModel> CreateAsync(MessageModel messageData, CancellationToken cancellationToken = default);
    }
}
