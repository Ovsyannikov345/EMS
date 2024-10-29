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
    }
}
