using ChatService.DAL.Models.Entities;

namespace ChatService.DAL.Repositories.IRepositories
{
    public interface IChatRepository : IGenericRepository<Chat>
    {
        public Task<Chat?> GetChatWithMessages(Guid chatId, CancellationToken cancellationToken = default);
    }
}
