using AutoMapper;
using ChatService.BLL.Models;
using ChatService.BLL.Services.IServices;
using ChatService.DAL.Models.Entities;
using ChatService.DAL.Repositories.IRepositories;

namespace ChatService.BLL.Services
{
    public class MessageService(IMessageRepository messageRepository, IMapper mapper) : IMessageService
    {
        public async Task<MessageModel> CreateAsync(MessageModel messageData, CancellationToken cancellationToken = default)
        {
            var messageToCreate = mapper.Map<Message>(messageData);

            var createdMessage = await messageRepository.CreateAsync(messageToCreate, cancellationToken);

            return mapper.Map<MessageModel>(createdMessage);
        }
    }
}
