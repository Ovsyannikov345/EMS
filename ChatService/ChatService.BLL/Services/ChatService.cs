using AutoMapper;
using ChatService.BLL.Models;
using ChatService.BLL.Services.IServices;
using ChatService.BLL.Utilities.Exceptions;
using ChatService.BLL.Utilities.Exceptions.Messages;
using ChatService.DAL.Grpc.Services.Estate;
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
        public async Task<ChatModel> GetChatAsync(Guid id, string userAuth0Id, CancellationToken cancellationToken = default)
        {
            var chat = await chatRepository.GetChatWithMessages(id, cancellationToken)
                ?? throw new NotFoundException(ExceptionMessages.NotFound(nameof(Chat), nameof(Chat.Id), id));

            var estateResponse = await estateGrpcClient.GetEstateAsync(chat.EstateId, cancellationToken);

            var chatModel = mapper.Map<ChatModel>(chat);

            chatModel.Estate = mapper.Map<EstateModel>(estateResponse.Estate);

            var profileResponse = await profileGrpcClient.GetProfile(chat.UserId, cancellationToken);

            chatModel.User = mapper.Map<UserProfileModel>(profileResponse.Profile);

            if (userAuth0Id != chatModel.User.Auth0Id && userAuth0Id != chatModel.Estate.User.Auth0Id)
            {
                throw new ForbiddenException(ExceptionMessages.AccessDenied(nameof(Chat), chatModel.Id));
            }

            return chatModel;
        }

        public async Task<IEnumerable<ChatModel>> GetEstateChatListAsync(string currentUserAuth0Id, CancellationToken cancellationToken = default)
        {
            var profileResponse = await profileGrpcClient.GetOwnProfile(currentUserAuth0Id, cancellationToken);

            var currentUser = mapper.Map<UserProfileModel>(profileResponse.Profile)
                ?? throw new NotFoundException(ExceptionMessages.NotFound(nameof(UserProfileModel), nameof(UserProfileModel.Auth0Id), currentUserAuth0Id));

            var estateResponse = await estateGrpcClient.GetUserEstateAsync(currentUser.Id, cancellationToken);

            var estate = mapper.Map<IEnumerable<ProtoEstateModel>, IEnumerable<EstateModel>>(estateResponse.EstateList).ToDictionary(e => e.Id);

            var estateIds = estate.Keys.ToList();

            var chats = await chatRepository.GetAllAsync(c => estateIds.Contains(c.EstateId), cancellationToken);

            var chatModels = mapper.Map<IEnumerable<Chat>, IEnumerable<ChatModel>>(chats);

            foreach (var chatModel in chatModels)
            {
                chatModel.Estate = estate[chatModel.EstateId];

                profileResponse = await profileGrpcClient.GetProfile(chatModel.UserId, cancellationToken);

                chatModel.User = mapper.Map<UserProfileModel>(profileResponse.Profile);
            }

            return chatModels;
        }

        public async Task<IEnumerable<ChatModel>> GetUserChatListAsync(string userAuth0Id, CancellationToken cancellationToken = default)
        {
            var profileResponse = await profileGrpcClient.GetOwnProfile(userAuth0Id, cancellationToken);

            var currentUser = mapper.Map<UserProfileModel>(profileResponse.Profile)
                ?? throw new NotFoundException(ExceptionMessages.NotFound(nameof(ProtoProfileModel), nameof(ProtoProfileModel.Auth0Id), userAuth0Id));

            var chats = await chatRepository.GetAllAsync(c => c.UserId == currentUser.Id, cancellationToken);

            var chatModels = mapper.Map<IEnumerable<Chat>, IEnumerable<ChatModel>>(chats).ToList();

            var estateResponse = await estateGrpcClient.GetEstateListAsync(chats.Select(chat => chat.EstateId), cancellationToken);

            var estate = estateResponse.EstateList.ToDictionary(e => e.Id);

            for (var i = 0; i < chatModels.Count; i++)
            {
                chatModels[i].Estate = mapper.Map<EstateModel>(estate[chatModels[i].EstateId.ToString()]);

                profileResponse = await profileGrpcClient.GetProfile(Guid.Parse(estate[chatModels[i].EstateId.ToString()].UserId), cancellationToken);

                chatModels[i].Estate.User = mapper.Map<UserProfileModel>(profileResponse.Profile);
            }

            return chatModels;
        }

        public async Task<ChatModel> CreateChatAsync(string userAuth0Id, Guid estateId, CancellationToken cancellationToken = default)
        {
            var profile = await profileGrpcClient.GetOwnProfile(userAuth0Id, cancellationToken);

            if (profile.Profile is null)
            {
                throw new NotFoundException(ExceptionMessages.NotFound(nameof(ProtoProfileModel), nameof(ProtoProfileModel.Auth0Id), userAuth0Id));
            }

            var estate = await estateGrpcClient.GetEstateAsync(estateId, cancellationToken);

            if (estate.Estate is null)
            {
                throw new NotFoundException(ExceptionMessages.NotFound(nameof(ProtoEstateModel), nameof(ProtoEstateModel.Id), estateId));
            }

            if (estate.Estate.User.Id == profile.Profile.Id)
            {
                throw new BadRequestException(ExceptionMessages.SelfChatCreation);
            }

            if (await chatRepository.Exists(c => c.UserId == Guid.Parse(profile.Profile.Id) && c.EstateId == estateId, cancellationToken))
            {
                throw new BadRequestException(ExceptionMessages.AlreadyExists(nameof(Chat), nameof(Chat.UserId), profile.Profile.Id));
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
