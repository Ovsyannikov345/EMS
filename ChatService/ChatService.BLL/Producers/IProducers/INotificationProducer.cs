using MessageBus.Messages;

namespace ChatService.BLL.Producers.IProducers
{
    public interface INotificationProducer
    {
        Task SendNotification(CreateNotification notificationData, CancellationToken cancellationToken = default);
    }
}