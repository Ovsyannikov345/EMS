using ChatService.DAL.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatService.DAL.Data
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {
            if (Database.IsRelational())
            {
                Database.Migrate();
            }
        }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<Message> Messages { get; set; }
    }
}
