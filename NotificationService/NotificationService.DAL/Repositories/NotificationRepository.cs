using NotificationService.DAL.Data;
using NotificationService.DAL.Models.Entities;
using NotificationService.DAL.Repositories.IRepositories;

namespace NotificationService.DAL.Repositories
{
    public class NotificationRepository(NotificationDbContext context) : GenericRepository<Notification>(context), INotificationRepository;
}
