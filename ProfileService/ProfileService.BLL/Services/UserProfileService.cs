using AutoMapper;
using ProfileService.BLL.Dto;
using ProfileService.BLL.Services.IServices;
using ProfileService.BLL.Utilities.Exceptions;
using ProfileService.BLL.Utilities.Messages;
using ProfileService.DAL.Models;
using ProfileService.DAL.Repositories.IRepositories;

namespace ProfileService.BLL.Services
{
    public class UserProfileService(IMapper mapper, IProfileRepository profileRepository) : IUserProfileService
    {
        public async Task<UserProfile> CreateProfileAsync(UserRegistrationData userData, CancellationToken cancellationToken = default)
        {
            var userProfile = mapper.Map<UserProfile>(userData);

            if (await profileRepository.GetByFilterAsync(p => p.Auth0Id == userProfile.Auth0Id, cancellationToken) is not null)
            {
                throw new BadRequestException(UserProfileMessages.ProfileAlreadyExists);
            }

            return await profileRepository.CreateAsync(userProfile, cancellationToken);
        }

        public async Task<UserProfile> GetProfileAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var userProfile = await profileRepository.GetByIdAsync(id, cancellationToken);

            if (userProfile is null)
            {
                throw new NotFoundException(UserProfileMessages.ProfileNotFound);
            }

            return userProfile;
        }

        public async Task<UserProfile> GetOwnProfileAsync(string auth0Id, CancellationToken cancellationToken = default)
        {
            var userProfile = await profileRepository.GetByFilterAsync(p => p.Auth0Id == auth0Id, cancellationToken);

            if (userProfile is null)
            {
                throw new NotFoundException(UserProfileMessages.ProfileNotFound);
            }

            return userProfile;
        }
    }
}
