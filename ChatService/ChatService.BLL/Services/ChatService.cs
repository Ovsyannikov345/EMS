using AutoMapper;
using ChatService.BLL.Models;
using ChatService.BLL.Services.IServices;
using ChatService.BLL.Utilities.Exceptions;
using ChatService.BLL.Utilities.Messages;
using ChatService.DAL.Grpc.Services.IServices;
using ChatService.DAL.Models.Entities;
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
            var chat = await chatRepository.GetChatWithMessages(id, cancellationToken)
                ?? throw new NotFoundException(ChatMessages.ChatNotFound);

            var estateResponse = await estateGrpcClient.GetEstateAsync(chat.EstateId, cancellationToken);

            var chatModel = mapper.Map<ChatModel>(chat);

            chatModel.Estate = mapper.Map<EstateModel>(estateResponse.Estate);

            var profileResponse = await profileGrpcClient.GetProfile(chat.UserId, cancellationToken);

            chatModel.User = mapper.Map<UserProfileModel>(profileResponse.Profile);

            return chatModel;
        }

        public async Task<IEnumerable<ChatModel>> GetEstateChatListAsync(Guid estateId, string currentUserAuth0Id, CancellationToken cancellationToken = default)
        {
            var profileResponse = await profileGrpcClient.GetOwnProfile(currentUserAuth0Id, cancellationToken);

            var currentUser = mapper.Map<UserProfileModel>(profileResponse.Profile) ?? throw new NotFoundException(ProfileMessages.ProfileNotFound);

            var estateResponse = await estateGrpcClient.GetEstateAsync(estateId, cancellationToken);

            var estate = mapper.Map<EstateModel>(estateResponse.Estate) ?? throw new NotFoundException(EstateMessages.EstateNotFound);

            if (estate.User.Id != currentUser.Id)
            {
                throw new ForbiddenException(ChatMessages.ChatListAccessDenied);
            }

            var chats = await chatRepository.GetAllAsync(c => c.EstateId == estateId, cancellationToken);

            var chatModels = mapper.Map<IEnumerable<Chat>, IEnumerable<ChatModel>>(chats);

            foreach (var chatModel in chatModels)
            {
                chatModel.Estate = estate;

                profileResponse = await profileGrpcClient.GetProfile(chatModel.UserId, cancellationToken);

                chatModel.User = mapper.Map<UserProfileModel>(profileResponse.Profile);
            }

            return chatModels;
        }

        public async Task<IEnumerable<ChatModel>> GetUserChatListAsync(string userAuth0Id, CancellationToken cancellationToken = default)
        {
            var profileResponse = await profileGrpcClient.GetOwnProfile(userAuth0Id, cancellationToken);

            var currentUser = mapper.Map<UserProfileModel>(profileResponse.Profile) ?? throw new NotFoundException(ProfileMessages.ProfileNotFound);

            var chats = await chatRepository.GetAllAsync(c => c.UserId == currentUser.Id, cancellationToken);

            var chatModels = mapper.Map<IEnumerable<Chat>, IEnumerable<ChatModel>>(chats);

            foreach (var chatModel in chatModels)
            {
                var estateResponse = await estateGrpcClient.GetEstateAsync(chatModel.EstateId, cancellationToken);

                chatModel.Estate = mapper.Map<EstateModel>(estateResponse.Estate);
            }

            return chatModels;
        }

        public async Task<ChatModel> CreateChatAsync(string userAuth0Id, Guid estateId, CancellationToken cancellationToken = default)
        {
            var profile = await profileGrpcClient.GetOwnProfile(userAuth0Id, cancellationToken);

            if (profile.Profile is null)
            {
                throw new NotFoundException(ProfileMessages.ProfileNotFound);
            }

            var estate = await estateGrpcClient.GetEstateAsync(estateId, cancellationToken);

            if (estate.Estate is null)
            {
                throw new NotFoundException(EstateMessages.EstateNotFound);
            }

            if (estate.Estate.User.Id == profile.Profile.Id)
            {
                throw new BadRequestException(ChatMessages.ChatWithYourself);
            }

            if (await chatRepository.Exists(c => c.UserId == Guid.Parse(profile.Profile.Id) && c.EstateId == estateId, cancellationToken))
            {
                throw new BadRequestException(ChatMessages.ChatAlreadyExists);
            }

            var createdChat = await chatRepository.CreateAsync(new Chat
            {
                UserId = Guid.Parse(profile.Profile.Id),
                EstateId = estateId,
                Messages = [],
            }, cancellationToken);

            return mapper.Map<ChatModel>(createdChat);
        }
    }
}
