using ChatService.DAL.Data;
using ChatService.DAL.Models.Entities;
using ChatService.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ChatService.DAL.Repositories
{
    public class ChatRepository(ChatDbContext context) : GenericRepository<Chat>(context), IChatRepository
    {
        public async Task<Chat?> GetChatWithMessages(Guid chatId, CancellationToken cancellationToken = default)
        {
            return await context.Chats.AsNoTracking().Include(c => c.Messages).FirstOrDefaultAsync(c => c.Id == chatId, cancellationToken);
        }
    }
}
