using Microsoft.EntityFrameworkCore;
using NotificationService.DAL.Models.Entities;

namespace NotificationService.DAL.Data
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions options) : base(options)
        {
            if (Database.IsRelational())
            {
                Database.Migrate();
            }
        }

        public DbSet<Notification> Notifications { get; set; }
    }
}
