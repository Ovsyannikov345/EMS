using AutoMapper;
using ProfileService.BLL.Dto;
using ProfileService.BLL.Services.IServices;
using ProfileService.BLL.Utilities.Exceptions;
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
                throw new BadRequestException("Profile already exists");
            }

            return await profileRepository.CreateAsync(userProfile, cancellationToken);
        }
    }
}
