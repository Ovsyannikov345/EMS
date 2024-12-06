using AutoFixture.Xunit2;
using AutoMapper;
using ChatService.BLL.Models;
using ChatService.BLL.Utilities.Exceptions;
using ChatService.BLL.Utilities.Mapping;
using ChatService.DAL.Grpc.Services.Estate;
using ChatService.DAL.Grpc.Services.IServices;
using ChatService.DAL.Grpc.Services.Profile;
using ChatService.DAL.Models.Entities;
using ChatService.DAL.Repositories.IRepositories;
using ChatService.Tests.DataInjection;
using ChatService.Tests.Mapping;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Linq.Expressions;

namespace ChatService.Tests.ServicesTests
{
    public class ChatServiceTests
    {
        private readonly Mapper _mapper = new(new MapperConfiguration(mc =>
            mc.AddProfiles([new AutoMapperProfile(), new TestMapperProfile()])));

        [Theory]
        [AutoDomainData]
        public async Task GetChatAsync_ChatNotExists_ThrowsNotFoundException(
            [Frozen] IChatRepository chatRepositoryMock,
            ChatModel chatModel,
            ProfileResponse profileResponse,
            ChatService.BLL.Services.ChatService sut)
        {
            // Arrange
            chatRepositoryMock.GetChatWithMessages(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .ReturnsNull();

            // Act
            var result = async () => await sut.GetChatAsync(chatModel.Id, profileResponse.Profile.Auth0Id, default);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task GetChatAsync_UserNotBelongsToChat_ThrowsForbiddenException(
            [Frozen] IChatRepository chatRepositoryMock,
            [Frozen] IEstateGrpcClient estateGrpcClientMock,
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            ChatModel chatModel,
            UserProfileModel userProfileModel,
            ProfileResponse profileResponse,
            EstateResponse estateResponse,
            ChatService.BLL.Services.ChatService sut)
        {
            // Arrange
            chatRepositoryMock.GetChatWithMessages(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(_mapper.Map<Chat>(chatModel));
            profileGrpcClientMock.GetProfile(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(profileResponse);
            estateGrpcClientMock.GetEstateAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(estateResponse);

            // Act
            var result = async () => await sut.GetChatAsync(chatModel.Id, userProfileModel.Auth0Id, default);

            // Assert
            await result.Should().ThrowAsync<ForbiddenException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task GetChatAsync_UserBelongsToChat_ReturnsChat(
            [Frozen] IChatRepository chatRepositoryMock,
            [Frozen] IEstateGrpcClient estateGrpcClientMock,
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            ChatModel chatModel,
            ProfileResponse profileResponse,
            EstateResponse estateResponse,
            ChatService.BLL.Services.ChatService sut)
        {
            // Arrange
            chatModel.UserId = Guid.Parse(profileResponse.Profile.Id);
            chatModel.User = _mapper.Map<UserProfileModel>(profileResponse.Profile);
            chatModel.Estate = _mapper.Map<EstateModel>(estateResponse.Estate);

            chatRepositoryMock.GetChatWithMessages(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(_mapper.Map<Chat>(chatModel));
            profileGrpcClientMock.GetProfile(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(profileResponse);
            estateGrpcClientMock.GetEstateAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(estateResponse);

            // Act
            var result = await sut.GetChatAsync(chatModel.Id, profileResponse.Profile.Auth0Id, default);

            // Assert
            result.Should().BeEquivalentTo(chatModel);
        }

        [Theory]
        [AutoDomainData]
        public async Task GetEstateChatListAsync_UserNotExists_ThrowsNotFoundException(
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            ProfileResponse profileResponse,
            ChatService.BLL.Services.ChatService sut)
        {
            // Arrange
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(new ProfileResponse());

            // Act
            var result = async () => await sut.GetEstateChatListAsync(profileResponse.Profile.Auth0Id, default);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task GetEstateChatListAsync_EstateNotExists_ThrowsNotFoundException(
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            [Frozen] IEstateGrpcClient estateGrpcClientMock,
            ProfileResponse profileResponse,
            ChatService.BLL.Services.ChatService sut)
        {
            // Arrange
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(profileResponse);
            estateGrpcClientMock.GetEstateAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(new EstateResponse());

            // Act
            var result = async () => await sut.GetEstateChatListAsync(profileResponse.Profile.Auth0Id, default);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task GetEstateChatListAsync_OtherPersonsEstate_ThrowsForbiddenException(
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            [Frozen] IEstateGrpcClient estateGrpcClientMock,
            EstateResponse estateResponse,
            ProfileResponse profileResponse,
            ChatService.BLL.Services.ChatService sut)
        {
            // Arrange
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(profileResponse);
            estateGrpcClientMock.GetEstateAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(estateResponse);

            // Act
            var result = async () => await sut.GetEstateChatListAsync(profileResponse.Profile.Auth0Id, default);

            // Assert
            await result.Should().ThrowAsync<ForbiddenException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task GetEstateChatListAsync_ValidEstate_ReturnsChatList(
            [Frozen] IChatRepository chatRepositoryMock,
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            [Frozen] IEstateGrpcClient estateGrpcClientMock,
            List<ChatModel> chatModels,
            EstateResponse estateResponse,
            ProfileResponse profileResponse,
            ChatService.BLL.Services.ChatService sut)
        {
            // Arrange
            estateResponse.Estate.User = _mapper.Map<DAL.Grpc.Services.Estate.ProtoProfileModel>(profileResponse.Profile);

            for (var i = 0; i < chatModels.Count; i++)
            {
                chatModels[i].Estate = _mapper.Map<EstateModel>(estateResponse.Estate);
                chatModels[i].User = _mapper.Map<UserProfileModel>(profileResponse.Profile);
            }

            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(profileResponse);
            profileGrpcClientMock.GetProfile(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(profileResponse);
            estateGrpcClientMock.GetEstateAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(estateResponse);
            chatRepositoryMock.GetAllAsync(Arg.Any<Expression<Func<Chat, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(_mapper.Map<IEnumerable<ChatModel>, IEnumerable<Chat>>(chatModels));

            // Act
            var result = await sut.GetEstateChatListAsync(profileResponse.Profile.Auth0Id, default);

            // Assert
            result.Should().BeEquivalentTo(chatModels);
        }

        [Theory]
        [AutoDomainData]
        public async Task GetUserChatListAsync_UserNotExists_ThrowsNotFoundException(
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            ProfileResponse profileResponse,
            ChatService.BLL.Services.ChatService sut)
        {
            // Arrange
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(new ProfileResponse());

            // Act
            var result = async () => await sut.GetUserChatListAsync(profileResponse.Profile.Auth0Id, default);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task GetUserChatListAsync_ValidUser_ReturnsChatList(
            [Frozen] IChatRepository chatRepositoryMock,
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            [Frozen] IEstateGrpcClient estateGrpcClientMock,
            List<ChatModel> chatModels,
            EstateResponse estateResponse,
            ProfileResponse profileResponse,
            ChatService.BLL.Services.ChatService sut)
        {
            // Arrange

            for (var i = 0; i < chatModels.Count; i++)
            {
                chatModels[i].Estate = _mapper.Map<EstateModel>(estateResponse.Estate);
                chatModels[i].User = null!;
            }

            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(profileResponse);
            estateGrpcClientMock.GetEstateAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(estateResponse);
            chatRepositoryMock.GetAllAsync(Arg.Any<Expression<Func<Chat, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(_mapper.Map<IEnumerable<ChatModel>, IEnumerable<Chat>>(chatModels));

            // Act
            var result = await sut.GetUserChatListAsync(profileResponse.Profile.Auth0Id, default);

            // Assert
            result.Should().BeEquivalentTo(chatModels);
        }

        [Theory]
        [AutoDomainData]
        public async Task CreateChatAsync_UserNotExists_ThrowsNotFoundException(
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            EstateModel estateModel,
            ProfileResponse profileResponse,
            ChatService.BLL.Services.ChatService sut)
        {
            // Arrange
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(new ProfileResponse());

            // Act
            var result = async () => await sut.CreateChatAsync(profileResponse.Profile.Auth0Id, estateModel.Id, default);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task CreateChatAsync_EstateNotExists_ThrowsNotFoundException(
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            [Frozen] IEstateGrpcClient estateGrpcClientMock,
            EstateModel estateModel,
            ProfileResponse profileResponse,
            ChatService.BLL.Services.ChatService sut)
        {
            // Arrange
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(profileResponse);
            estateGrpcClientMock.GetEstateAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(new EstateResponse());

            // Act
            var result = async () => await sut.CreateChatAsync(profileResponse.Profile.Auth0Id, estateModel.Id, default);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task CreateChatAsync_ChatWithYourself_ThrowsBadRequestException(
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            [Frozen] IEstateGrpcClient estateGrpcClientMock,
            EstateModel estateModel,
            ProfileResponse profileResponse,
            EstateResponse estateResponse,
            ChatService.BLL.Services.ChatService sut)
        {
            // Arrange
            estateResponse.Estate.User = _mapper.Map<DAL.Grpc.Services.Estate.ProtoProfileModel>(profileResponse.Profile);

            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(profileResponse);
            estateGrpcClientMock.GetEstateAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(estateResponse);

            // Act
            var result = async () => await sut.CreateChatAsync(profileResponse.Profile.Auth0Id, estateModel.Id, default);

            // Assert
            await result.Should().ThrowAsync<BadRequestException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task CreateChatAsync_ChatExists_ThrowsBadRequestException(
            [Frozen] IChatRepository chatRepositoryMock,
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            [Frozen] IEstateGrpcClient estateGrpcClientMock,
            EstateModel estateModel,
            ProfileResponse profileResponse,
            EstateResponse estateResponse,
            ChatService.BLL.Services.ChatService sut)
        {
            // Arrange
            chatRepositoryMock.Exists(Arg.Any<Expression<Func<Chat, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(true);
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(profileResponse);
            estateGrpcClientMock.GetEstateAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(estateResponse);

            // Act
            var result = async () => await sut.CreateChatAsync(profileResponse.Profile.Auth0Id, estateModel.Id, default);

            // Assert
            await result.Should().ThrowAsync<BadRequestException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task CreateChatAsync_ValidChat_ReturnsCreatedChat(
            [Frozen] IChatRepository chatRepositoryMock,
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            [Frozen] IEstateGrpcClient estateGrpcClientMock,
            ChatModel chatModel,
            ProfileResponse profileResponse,
            EstateResponse estateResponse,
            ChatService.BLL.Services.ChatService sut)
        {
            // Arrange
            chatModel.User = null!;
            chatModel.Estate = null!;

            chatRepositoryMock.Exists(Arg.Any<Expression<Func<Chat, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(false);
            chatRepositoryMock.CreateAsync(Arg.Any<Chat>(), Arg.Any<CancellationToken>())
                .Returns(_mapper.Map<Chat>(chatModel));
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(profileResponse);
            estateGrpcClientMock.GetEstateAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(estateResponse);

            // Act
            var result = await sut.CreateChatAsync(profileResponse.Profile.Auth0Id, Guid.Parse(estateResponse.Estate.Id), default);

            // Assert
            result.Should().BeEquivalentTo(chatModel);
        }
    }
}
