using MassTransit;
using NotificationService.Consumers.Messages;

namespace NotificationService.Consumers
{
    public class CreateNotificationConsumer : IConsumer<CreateNotification>
    {
        public Task Consume(ConsumeContext<CreateNotification> context)
        {
            throw new NotImplementedException();
        }
    }
}
