using ChatService.BLL.Producers.IProducers;
using MassTransit;
using MessageBus.Messages;

namespace ChatService.BLL.Producers
{
    public class NotificationProducer(ISendEndpointProvider sendEndpointProvider) : INotificationProducer
    {
        public async Task SendNotification(CreateNotification notificationData, CancellationToken cancellationToken = default)
        {
            var endpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:CreateNotification"));

            await endpoint.Send(notificationData, cancellationToken);
        }
    }
}
