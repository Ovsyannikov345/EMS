using NotificationService.DAL.Models.Entities;
using System.Linq.Expressions;

namespace NotificationService.DAL.Repositories.IRepositories
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetAllAsync<TSortKey>(
            Expression<Func<Notification, bool>>? predicate = null,
            Expression<Func<Notification, TSortKey>>? sortSelector = null,
            bool isDescending = false,
            CancellationToken cancellationToken = default);
    }
}
