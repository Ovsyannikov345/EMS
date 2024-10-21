using Microsoft.EntityFrameworkCore;
using NotificationService.DAL.Models.Entities;

namespace NotificationService.DAL.Data
{
    public class NotificationDbContext(DbContextOptions<NotificationDbContext> context) : DbContext(context)
    {
        public DbSet<Notification> Notifications { get; set; }
    }
}
