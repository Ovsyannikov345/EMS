using AutoMapper;
using ProfileService.BLL.Models;
using ProfileService.BLL.Services.IServices;
using ProfileService.BLL.Utilities.Exceptions;
using ProfileService.BLL.Utilities.Exceptions.Messages;
using ProfileService.DAL.CacheRepositoryManagers.ICacheRepositoryManagers;
using ProfileService.DAL.Models;

namespace ProfileService.BLL.Services
{
    public class ProfileInfoVisibilityService(
        ICacheRepositoryManager<UserProfile> profileCacheRepositoryManager,
        ICacheRepositoryManager<ProfileInfoVisibility> visibilityCacheRepositoryManager,
        IMapper mapper) : IProfileInfoVisibilityService
    {
        public async Task<ProfileInfoVisibilityModel> GetProfileInfoVisibilityAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var visibilityKey = nameof(ProfileInfoVisibility) + userId;

            var visibility = await visibilityCacheRepositoryManager.GetEntityByFilterAsync(
                visibilityKey, v => v.UserId == userId, cancellationToken: cancellationToken)
                ?? throw new NotFoundException(ExceptionMessages.NotFound(nameof(ProfileInfoVisibility), nameof(ProfileInfoVisibility.UserId), userId));

            return mapper.Map<ProfileInfoVisibilityModel>(visibility);
        }

        public async Task<ProfileInfoVisibilityModel> UpdateProfileInfoVisibilityAsync(string currentUserAuth0Id, Guid userId, ProfileInfoVisibilityModel visibilityData, CancellationToken cancellationToken = default)
        {
            if (userId != visibilityData.UserId)
            {
                throw new BadRequestException(ExceptionMessages.InvalidId(nameof(ProfileInfoVisibilityModel), userId));
            }

            var profile = await profileCacheRepositoryManager.GetEntityByIdAsync(userId, cancellationToken: cancellationToken)
                ?? throw new NotFoundException(ExceptionMessages.NotFound(nameof(UserProfile), nameof(UserProfile.Auth0Id), currentUserAuth0Id));

            if (profile.Id != visibilityData.UserId)
            {
                throw new ForbiddenException(ExceptionMessages.AccessDenied(nameof(UserProfile), profile.Id));
            }

            var updatedVisibility = await visibilityCacheRepositoryManager.UpdateEntityAsync(
                mapper.Map<ProfileInfoVisibility>(visibilityData), [nameof(ProfileInfoVisibility) + userId], cancellationToken);

            return mapper.Map<ProfileInfoVisibilityModel>(updatedVisibility);
        }
    }
}
