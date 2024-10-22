using AutoMapper;
using MassTransit;
using NotificationService.BLL.Models;
using NotificationService.BLL.Services.IServices;
using NotificationService.Consumers.Messages;

namespace NotificationService.Consumers
{
    public class CreateNotificationConsumer(INotificationService notificationService, IMapper mapper) : IConsumer<CreateNotification>
    {
        public async Task Consume(ConsumeContext<CreateNotification> context)
        {
            var notificationToCreate = mapper.Map<NotificationModel>(context.Message);

            await notificationService.CreateNotificationAsync(notificationToCreate);
        }
    }
}
