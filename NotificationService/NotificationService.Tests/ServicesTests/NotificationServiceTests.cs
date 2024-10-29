using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using NotificationService.BLL.Models;
using NotificationService.BLL.Utilities.Mapping;
using NotificationService.DAL.Models.Entities;
using NotificationService.DAL.Repositories.IRepositories;
using NotificationService.Tests.DataInjection;
using NSubstitute;

namespace NotificationService.Tests.ServicesTests
{
    public class NotificationServiceTests
    {
        private readonly Mapper _mapper = new(new MapperConfiguration(mc =>
            mc.AddProfile(new AutoMapperProfile())));

        [Theory]
        [AutoDomainData]
        public async Task CreateNotificationAsync_ValidNotification_ReturnsCreatedNotification(
            [Frozen] INotificationRepository notificationRepositoryMock,
            NotificationModel notificationModel,
            NotificationService.BLL.Services.NotificationService sut)
        {
            // Arrange
            notificationRepositoryMock.CreateAsync(Arg.Any<Notification>(), Arg.Any<CancellationToken>())
                .Returns(_mapper.Map<Notification>(notificationModel));

            // Act
            var result = await sut.CreateNotificationAsync(notificationModel, default);

            // Assert
            result.Should().BeEquivalentTo(notificationModel);
        }
    }
}
