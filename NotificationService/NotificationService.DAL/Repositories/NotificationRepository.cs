using NotificationService.DAL.Data;
using NotificationService.DAL.Models.Entities;
using NotificationService.DAL.Repositories.IRepositories;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace NotificationService.DAL.Repositories
{
    public class NotificationRepository(NotificationDbContext context) : GenericRepository<Notification>(context), INotificationRepository
    {
        public async Task<IEnumerable<Notification>> GetAllAsync<TSortKey>(
            Expression<Func<Notification, bool>>? predicate = null,
            Expression<Func<Notification, TSortKey>>? sortSelector = null,
            bool isDescending = false,
            CancellationToken cancellationToken = default)
        {
            var entities = predicate switch
            {
                null => context.Set<Notification>(),
                _ => context.Set<Notification>().Where(predicate),
            };

            if (sortSelector is not null)
            {
                entities = isDescending ? entities.OrderByDescending(sortSelector) : entities.OrderBy(sortSelector);
            }

            return await entities.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
