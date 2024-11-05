using AutoFixture.Xunit2;
using AutoMapper;
using ChatService.BLL.Models;
using ChatService.BLL.Producers.IProducers;
using ChatService.BLL.Services;
using ChatService.BLL.Utilities.Mapping;
using ChatService.DAL.Models.Entities;
using ChatService.DAL.Repositories.IRepositories;
using ChatService.Tests.DataInjection;
using FluentAssertions;
using NSubstitute;

namespace ChatService.Tests.ServicesTests
{
    public class MessageServiceTests
    {
        private readonly IMapper _mapper = new Mapper(new MapperConfiguration(mc =>
            mc.AddProfile(new AutoMapperProfile())));

        [Theory]
        [AutoDomainData]
        public async Task CreateAsync_ValidMessage_ReturnsCreatedMessage(
            [Frozen] IMessageRepository messageRepositoryMock,
            MessageModel message,
            MessageService sut)
        {
            // Arrange
            messageRepositoryMock.CreateAsync(Arg.Any<Message>(), Arg.Any<CancellationToken>())
                .Returns(_mapper.Map<Message>(message));

            // Act
            var result = await sut.CreateAsync(message, default);

            // Assert
            result.Should().BeEquivalentTo(message);
        }
    }
}
