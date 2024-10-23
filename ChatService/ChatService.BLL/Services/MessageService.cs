using AutoMapper;
using ChatService.BLL.Models;
using ChatService.BLL.Producers.IProducers;
using ChatService.BLL.Services.IServices;
using ChatService.BLL.Utilities.Messages;
using ChatService.DAL.Models.Entities;
using ChatService.DAL.Repositories.IRepositories;
using MessageBus.Messages;

namespace ChatService.BLL.Services
{
    public class MessageService(
        IMessageRepository messageRepository,
        INotificationProducer notificationProducer,
        IMapper mapper) : IMessageService
    {
        public async Task<MessageModel> CreateAsync(MessageModel messageData, CancellationToken cancellationToken = default)
        {
            var messageToCreate = mapper.Map<Message>(messageData);

            var createdMessage = await messageRepository.CreateAsync(messageToCreate, cancellationToken);

            await notificationProducer.SendNotification(new CreateNotification
            {
                UserId = messageData.UserId,
                Title = NotificationMessages.NewMessage,
            }, cancellationToken);

            return mapper.Map<MessageModel>(createdMessage);
        }
    }
}
