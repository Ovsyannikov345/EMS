using AutoMapper;
using ChatService.BLL.Models;
using ChatService.BLL.Services.IServices;
using ChatService.BLL.Utilities.Exceptions;
using ChatService.BLL.Utilities.Messages;
using ChatService.DAL.Grpc.Services.IServices;
using ChatService.DAL.Repositories.IRepositories;

namespace ChatService.BLL.Services
{
    public class ChatService(
        IChatRepository chatRepository,
        IEstateGrpcClient estateGrpcClient,
        IProfileGrpcClient profileGrpcClient,
        IMapper mapper) : IChatService
    {
        public async Task<ChatModel> GetChatAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var chat = await chatRepository.GetChatWithMessages(id, cancellationToken);

            if (chat == null)
            {
                throw new NotFoundException(ChatMessages.ChatNotFound);
            }

            var estateResponse = await estateGrpcClient.GetEstateAsync(chat.EstateId, cancellationToken);

            var chatModel = mapper.Map<ChatModel>(chat);

            chatModel.Estate = mapper.Map<EstateModel>(estateResponse.Estate);

            var profileResponse = await profileGrpcClient.GetProfile(chat.UserId, cancellationToken);

            chatModel.User = new UserProfileModel
            {
                Id = Guid.Parse(profileResponse.Profile.Id),
                Auth0Id = profileResponse.Profile.Auth0Id,
                FirstName = profileResponse.Profile.FirstName,
                LastName = profileResponse.Profile.LastName,
                PhoneNumber = profileResponse.Profile.PhoneNumber,
                BirthDate = profileResponse.Profile.BirthDate.ToDateTime()
            };

            return chatModel;
        }
    }
}
