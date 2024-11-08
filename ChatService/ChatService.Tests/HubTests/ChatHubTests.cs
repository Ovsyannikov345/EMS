using AutoFixture.Xunit2;
using AutoMapper;
using ChatService.BLL.Models;
using ChatService.BLL.Services.IServices;
using ChatService.DAL.Grpc.Services.IServices;
using ChatService.DAL.Grpc.Services.Profile;
using ChatService.Hubs;
using ChatService.Tests.DataInjection;
using ChatService.Tests.Mapping;
using ChatService.ViewModels;
using Microsoft.AspNetCore.SignalR;
using NSubstitute;
using System.Security.Claims;

namespace ChatService.Tests.HubTests
{
    public class ChatHubTests
    {
        private readonly Mapper _mapper = new(new MapperConfiguration(mc =>
            mc.AddProfiles([
                new BLL.Utilities.Mapping.AutoMapperProfile(),
                new Utilities.Mapping.AutoMapperProfile(),
                new TestMapperProfile()])));

        [Theory]
        [AutoDomainData]
        public async Task Send_ValidMessage_ClientReceivesCreatedMessage(
            [Frozen] IChatService chatServiceMock,
            [Frozen] IMessageService messageServiceMock,
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            [Frozen] IHubCallerClients clientsMock,
            [Frozen] HubCallerContext hubCallerContextMock,
            ISingleClientProxy clientProxyMock,
            ChatModel chat,
            MessageModel message,
            ProfileResponse profileResponse,
            ChatHub sut)
        {
            // Arrange
            chat.User = _mapper.Map<UserProfileModel>(profileResponse.Profile);
            chat.UserId = Guid.Parse(profileResponse.Profile.Id);
            chat.Estate.User = _mapper.Map<UserProfileModel>(profileResponse.Profile);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, profileResponse.Profile.Auth0Id),
            };
            var identity = new ClaimsIdentity(claims, "mock");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            clientsMock.All
                .Returns(clientProxyMock);
            clientsMock.Client(Arg.Any<string>())
                .Returns(clientProxyMock);
            hubCallerContextMock.User
                .Returns(claimsPrincipal);
            hubCallerContextMock.ConnectionId
                .Returns(profileResponse.Profile.Auth0Id);
            chatServiceMock.GetChatAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(chat);
            messageServiceMock.CreateAsync(Arg.Any<MessageModel>(), Arg.Any<CancellationToken>())
                .Returns(message);
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(profileResponse);

            // Act
            await sut.OnConnectedAsync();
            await sut.Send(chat.Id, message.Text);
            await sut.OnDisconnectedAsync(new Exception());

            // Assert
            await clientProxyMock.Received().SendCoreAsync(
            "Receive",
            Arg.Is<object[]>(args =>
                args.Length == 1 &&
                (args[0] is MessageViewModel) &&
                (args[0] as MessageViewModel) == (_mapper.Map<MessageViewModel>(message))),
            default);
        }
    }
}
