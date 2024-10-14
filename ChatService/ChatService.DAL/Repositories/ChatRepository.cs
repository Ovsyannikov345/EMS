using ChatService.DAL.Data;
using ChatService.DAL.Models.Entities;
using ChatService.DAL.Repositories.IRepositories;

namespace ChatService.DAL.Repositories
{
    public class ChatRepository(ChatDbContext context) : GenericRepository<Chat>(context), IChatRepository;
}
