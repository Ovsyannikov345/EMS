using AutoMapper;
using NotificationService.BLL.Models;
using NotificationService.BLL.Services.IServices;
using NotificationService.BLL.Utilities.Exceptions;
using NotificationService.BLL.Utilities.Exceptions.Messages;
using NotificationService.DAL.Grpc.Services.IServices;
using NotificationService.DAL.Grpc.Services.Profile;
using NotificationService.DAL.Models.Entities;
using NotificationService.DAL.Repositories.IRepositories;

namespace NotificationService.BLL.Services
{
    public class NotificationService(
        INotificationRepository notificationRepository,
        IProfileGrpcClient profileGrpcClient,
        IMapper mapper) : INotificationService
    {
        public async Task<NotificationModel> CreateNotificationAsync(NotificationModel notificationData, CancellationToken cancellationToken = default)
        {
            var notification = await notificationRepository.CreateAsync(mapper.Map<Notification>(notificationData), cancellationToken);

            return mapper.Map<NotificationModel>(notification);
        }

        public async Task<IEnumerable<NotificationModel>> GetNotificationListAsync(string userAuth0Id, CancellationToken cancellationToken = default)
        {
            var profile = await profileGrpcClient.GetOwnProfile(userAuth0Id, cancellationToken)
                ?? throw new NotFoundException(ExceptionMessages.NotFound(nameof(ProfileResponse.Profile), nameof(ProfileResponse.Profile.Auth0Id), userAuth0Id));

            var notificationList = await notificationRepository.GetAllAsync(n => n.UserId == Guid.Parse(profile.Profile.Id), cancellationToken);

            return mapper.Map<IEnumerable<Notification>, IEnumerable<NotificationModel>>(notificationList);
        }

        public async Task<NotificationModel> MarkNotificationAsReadAsync(Guid notificationId, string userAuth0Id, CancellationToken cancellationToken = default)
        {
            var profile = await profileGrpcClient.GetOwnProfile(userAuth0Id, cancellationToken)
                ?? throw new NotFoundException(ExceptionMessages.NotFound(nameof(ProfileResponse.Profile), nameof(ProfileResponse.Profile.Auth0Id), userAuth0Id));

            var notification = await notificationRepository.GetByFilterAsync(n => n.Id == notificationId, cancellationToken)
                ?? throw new NotFoundException(ExceptionMessages.NotFound(nameof(Notification), nameof(Notification.Id), notificationId));

            if (notification.UserId != Guid.Parse(profile.Profile.Id))
            {
                throw new ForbiddenException(ExceptionMessages.AccessDenied(nameof(Notification), notificationId));
            }

            notification.IsRead = true;
            await notificationRepository.UpdateAsync(notification, cancellationToken);

            return mapper.Map<NotificationModel>(notification);
        }
    }
}
