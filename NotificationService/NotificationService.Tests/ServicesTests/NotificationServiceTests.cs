﻿using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using NotificationService.BLL.Models;
using NotificationService.BLL.Utilities.Exceptions;
using NotificationService.BLL.Utilities.Mapping;
using NotificationService.DAL.Grpc.Services.IServices;
using NotificationService.DAL.Grpc.Services.Profile;
using NotificationService.DAL.Models.Entities;
using NotificationService.DAL.Repositories.IRepositories;
using NotificationService.Tests.DataInjection;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Linq.Expressions;

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

        [Theory]
        [AutoDomainData]
        public async Task GetNotificationListAsync_ProfileNotExists_ThrowsNotFoundException(
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            NotificationService.BLL.Services.NotificationService sut)
        {
            // Arrange
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .ReturnsNull();

            // Act
            var result = async () => await sut.GetNotificationListAsync("", default);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task GetNotificationListAsync_ValidProfile_ReturnsNotificationList(
            [Frozen] INotificationRepository notificationRepositoryMock,
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            List<NotificationModel> notificationModelList,
            ProfileResponse profileResponse,
            NotificationService.BLL.Services.NotificationService sut)
        {
            // Arrange
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(profileResponse);
            notificationRepositoryMock.GetAllAsync(Arg.Any<Expression<Func<Notification, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(_mapper.Map<IEnumerable<NotificationModel>, IEnumerable<Notification>>(notificationModelList));

            // Act
            var result = await sut.GetNotificationListAsync(profileResponse.Profile.Auth0Id, default);

            // Assert
            result.Should().BeEquivalentTo(notificationModelList);
        }

        [Theory]
        [AutoDomainData]
        public async Task MarkNotificationAsReadAsync_ProfileNotExists_ThrowsNotFoundException(
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            NotificationModel notificationModel,
            ProfileResponse profileResponse,
            NotificationService.BLL.Services.NotificationService sut)
        {
            // Arrange
            profileResponse.Profile = null;

            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(profileResponse);

            // Act
            var result = async () => await sut.MarkNotificationAsReadAsync(notificationModel.Id, "", default);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task MarkNotificationAsReadAsync_NotificationNotExists_ThrowsNotFoundException(
            [Frozen] INotificationRepository notificationRepositoryMock,
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            NotificationModel notificationModel,
            ProfileResponse profileResponse,
            NotificationService.BLL.Services.NotificationService sut)
        {
            // Arrange
            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(profileResponse);
            notificationRepositoryMock.GetByFilterAsync(Arg.Any<Expression<Func<Notification, bool>>>(), Arg.Any<CancellationToken>())
                .ReturnsNull();

            // Act
            var result = async () => await sut.MarkNotificationAsReadAsync(notificationModel.Id, profileResponse.Profile.Auth0Id, default);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task MarkNotificationAsReadAsync_OtherPersonsNotification_ThrowsForbiddenException(
            [Frozen] INotificationRepository notificationRepositoryMock,
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            NotificationModel notificationModel,
            ProfileResponse profileResponse,
            NotificationService.BLL.Services.NotificationService sut)
        {
            // Arrange
            profileResponse.Profile.Id = Guid.NewGuid().ToString();

            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(profileResponse);
            notificationRepositoryMock.GetByFilterAsync(Arg.Any<Expression<Func<Notification, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(_mapper.Map<Notification>(notificationModel));

            // Act
            var result = async () => await sut.MarkNotificationAsReadAsync(notificationModel.Id, profileResponse.Profile.Auth0Id, default);

            // Assert
            await result.Should().ThrowAsync<ForbiddenException>();
        }

        [Theory]
        [AutoDomainData]
        public async Task MarkNotificationAsReadAsync_ValidNotification_ReturnsReadNotification(
            [Frozen] INotificationRepository notificationRepositoryMock,
            [Frozen] IProfileGrpcClient profileGrpcClientMock,
            NotificationModel notificationModel,
            ProfileResponse profileResponse,
            NotificationService.BLL.Services.NotificationService sut)
        {
            // Arrange
            profileResponse.Profile.Id = Guid.NewGuid().ToString();
            notificationModel.UserId = Guid.Parse(profileResponse.Profile.Id);

            var expected = new NotificationModel
            {
                Id = notificationModel.Id,
                UserId = notificationModel.UserId,
                Title = notificationModel.Title,
                CreatedAt = notificationModel.CreatedAt,
                IsRead = true
            };

            profileGrpcClientMock.GetOwnProfile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(profileResponse);
            notificationRepositoryMock.GetByFilterAsync(Arg.Any<Expression<Func<Notification, bool>>>(), Arg.Any<CancellationToken>())
                .Returns(_mapper.Map<Notification>(notificationModel));

            // Act
            var result = await sut.MarkNotificationAsReadAsync(notificationModel.Id, profileResponse.Profile.Auth0Id, default);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
