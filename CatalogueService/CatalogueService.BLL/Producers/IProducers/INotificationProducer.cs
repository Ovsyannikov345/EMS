using MessageBus.Messages;

namespace CatalogueService.BLL.Producers.IProducers
{
    public interface INotificationProducer
    {
        Task SendNotification(CreateNotification notificationData, CancellationToken cancellationToken = default);
    }
}