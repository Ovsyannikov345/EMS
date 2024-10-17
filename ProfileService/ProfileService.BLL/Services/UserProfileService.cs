using AutoMapper;
using ProfileService.BLL.Models;
using ProfileService.BLL.Services.IServices;
using ProfileService.BLL.Utilities.Exceptions;
using ProfileService.BLL.Utilities.Messages;
using ProfileService.DAL.Models;
using ProfileService.DAL.Repositories.IRepositories;

namespace ProfileService.BLL.Services
{
    public class UserProfileService(IMapper mapper, IProfileRepository profileRepository, IProfileInfoVisibilityRepository visibilityRepository) : IUserProfileService
    {
        public async Task<UserProfileModel> CreateProfileAsync(RegistrationDataModel userData, CancellationToken cancellationToken = default)
        {
            var userProfile = mapper.Map<UserProfile>(userData);

            if (await profileRepository.GetByFilterAsync(p => p.Auth0Id == userProfile.Auth0Id, cancellationToken) is not null)
            {
                throw new BadRequestException(UserProfileMessages.ProfileAlreadyExists);
            }

            var createdUser = await profileRepository.CreateAsync(userProfile, cancellationToken);

            await visibilityRepository.CreateAsync(new ProfileInfoVisibility { UserId = createdUser.Id, User = createdUser }, cancellationToken);

            return mapper.Map<UserProfileModel>(createdUser);
        }

        public async Task<UserProfileModel> GetProfileAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var userProfile = await profileRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException(UserProfileMessages.ProfileNotFound);

            return mapper.Map<UserProfileModel>(userProfile);
        }

        public async Task<UserProfileModel> GetOwnProfileAsync(string auth0Id, CancellationToken cancellationToken = default)
        {
            var userProfile = await profileRepository.GetByFilterAsync(p => p.Auth0Id == auth0Id, cancellationToken)
                ?? throw new NotFoundException(UserProfileMessages.ProfileNotFound);

            return mapper.Map<UserProfileModel>(userProfile);
        }

        public async Task<UserProfileModel> UpdateProfileAsync(UserProfileModel userData, string currentUserAuth0Id, CancellationToken cancellationToken = default)
        {
            var profile = await profileRepository.GetByFilterAsync(p => p.Id == userData.Id, cancellationToken)
                ?? throw new NotFoundException(UserProfileMessages.ProfileNotFound);

            if (currentUserAuth0Id != profile.Auth0Id)
            {
                throw new ForbiddenException(UserProfileMessages.AccessDenied);
            }

            var userToUpdate = mapper.Map<UserProfile>(userData);

            profile = await profileRepository.UpdateAsync(userToUpdate, cancellationToken);

            return mapper.Map<UserProfileModel>(profile);
        }
    }
}
