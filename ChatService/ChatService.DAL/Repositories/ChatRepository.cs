using ChatService.DAL.Data;
using ChatService.DAL.Models.Entities;
using ChatService.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ChatService.DAL.Repositories
{
    public class ChatRepository : GenericRepository<Chat>, IChatRepository
    {
        private readonly ChatDbContext _context;

        public ChatRepository(ChatDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Chat?> GetChatWithMessages(Guid chatId, CancellationToken cancellationToken = default)
        {
            return await _context.Chats.AsNoTracking().Include(c => c.Messages).FirstOrDefaultAsync(c => c.Id == chatId, cancellationToken);
        }
    }
}
