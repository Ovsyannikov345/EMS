using NotificationService.BLL.Models;

namespace NotificationService.BLL.Services.IServices
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationModel>> GetNotificationListAsync(string userAuth0Id, CancellationToken cancellationToken = default);

        Task<NotificationModel> CreateNotificationAsync(NotificationModel notificationData, CancellationToken cancellationToken = default);

        Task<NotificationModel> MarkNotificationAsReadAsync(Guid notificationId, string userAuth0Id, CancellationToken cancellationToken = default);
    }
}
