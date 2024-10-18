using AutoMapper;
using ProfileService.BLL.Models;
using ProfileService.BLL.Services.IServices;
using ProfileService.BLL.Utilities.Exceptions;
using ProfileService.BLL.Utilities.Exceptions.Messages;
using ProfileService.DAL.Models;
using ProfileService.DAL.Repositories.IRepositories;

namespace ProfileService.BLL.Services
{
    public class ProfileInfoVisibilityService(
        IProfileInfoVisibilityRepository visibilityRepository,
        IProfileRepository profileRepository,
        IMapper mapper) : IProfileInfoVisibilityService
    {
        public async Task<ProfileInfoVisibilityModel> GetProfileInfoVisibilityAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var visibility = await visibilityRepository.GetByFilterAsync(v => v.UserId == userId, cancellationToken)
                ?? throw new NotFoundException(ExceptionMessages.NotFound(nameof(ProfileInfoVisibility), nameof(ProfileInfoVisibility.UserId), userId));

            return mapper.Map<ProfileInfoVisibilityModel>(visibility);
        }

        public async Task<ProfileInfoVisibilityModel> UpdateProfileInfoVisibilityAsync(string currentUserAuth0Id, ProfileInfoVisibilityModel visibilityData, CancellationToken cancellationToken = default)
        {
            var profile = await profileRepository.GetByFilterAsync(p => p.Auth0Id == currentUserAuth0Id, cancellationToken)
                ?? throw new NotFoundException(ExceptionMessages.NotFound(nameof(UserProfile), nameof(UserProfile.Auth0Id), currentUserAuth0Id));

            if (profile.Id != visibilityData.UserId)
            {
                throw new ForbiddenException(ExceptionMessages.AccessDenied(nameof(UserProfile), profile.Id));
            }

            var updatedVisibility = await visibilityRepository.UpdateAsync(mapper.Map<ProfileInfoVisibility>(visibilityData), cancellationToken);

            return mapper.Map<ProfileInfoVisibilityModel>(updatedVisibility);
        }
    }
}
