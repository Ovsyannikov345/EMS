using AutoMapper;
using ProfileService.BLL.Models;
using ProfileService.BLL.Services.IServices;
using ProfileService.BLL.Utilities.Exceptions;
using ProfileService.BLL.Utilities.Exceptions.Messages;
using ProfileService.DAL.CacheProvider.ICacheProvider;
using ProfileService.DAL.Models;
using ProfileService.DAL.Models.Enums;
using ProfileService.DAL.Repositories.IRepositories;

namespace ProfileService.BLL.Services
{
    public class UserProfileService(
        IProfileRepository profileRepository,
        IProfileInfoVisibilityRepository visibilityRepository,
        IMapper mapper,
        IRedisCacheProvider redisCache) : IUserProfileService
    {
        public async Task<UserProfileModel> CreateProfileAsync(RegistrationDataModel userData, CancellationToken cancellationToken = default)
        {
            var userProfile = mapper.Map<UserProfile>(userData);

            if (await profileRepository.GetByFilterAsync(p => p.Auth0Id == userProfile.Auth0Id, cancellationToken) is not null)
            {
                throw new BadRequestException(ExceptionMessages.AlreadyExists(nameof(UserProfile), nameof(UserProfile.Auth0Id), userProfile.Auth0Id));
            }

            var createdUser = await profileRepository.CreateAsync(userProfile, cancellationToken);

            await visibilityRepository.CreateAsync(new ProfileInfoVisibility { UserId = createdUser.Id, User = createdUser }, cancellationToken);

            return mapper.Map<UserProfileModel>(createdUser);
        }

        public async Task<UserProfileModelWithPrivacy> GetProfileAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var profileKey = nameof(UserProfile) + id;

            var userProfile = await redisCache.GetDataFromCache<UserProfile>(profileKey);

            if (userProfile is null)
            {
                userProfile = await profileRepository.GetByFilterAsync(p => p.Id == id, cancellationToken)
                    ?? throw new NotFoundException(ExceptionMessages.NotFound(nameof(UserProfile), nameof(UserProfile.Id), id));

                await redisCache.CacheData(userProfile, TimeSpan.FromMinutes(10), profileKey);
            }

            var visibilityKey = nameof(ProfileInfoVisibility) + id;

            var visibilityOptions = await redisCache.GetDataFromCache<ProfileInfoVisibility>(visibilityKey);

            if (visibilityOptions is null)
            {
                visibilityOptions = await visibilityRepository.GetByFilterAsync(v => v.UserId == userProfile.Id, cancellationToken)
                    ?? throw new NotFoundException(ExceptionMessages.NotFound(nameof(ProfileInfoVisibility), nameof(ProfileInfoVisibility.UserId), userProfile.Id));

                await redisCache.CacheData(visibilityOptions, TimeSpan.FromMinutes(10), visibilityKey);
            }

            var profileModel = mapper.Map<UserProfileModelWithPrivacy>(userProfile);

            if (visibilityOptions.BirthDateVisibility == InfoVisibility.Private)
            {
                profileModel.BirthDate = null;
            }

            if (visibilityOptions.PhoneNumberVisibility == InfoVisibility.Private)
            {
                profileModel.PhoneNumber = null;
            }

            return profileModel;
        }

        public async Task<UserProfileModel> GetOwnProfileAsync(string auth0Id, CancellationToken cancellationToken = default)
        {
            var profileKey = nameof(UserProfile.Auth0Id) + auth0Id;

            var userProfile = await redisCache.GetDataFromCache<UserProfile>(profileKey);

            if (userProfile is null)
            {
                userProfile = await profileRepository.GetByFilterAsync(p => p.Auth0Id == auth0Id, cancellationToken)
                    ?? throw new NotFoundException(ExceptionMessages.NotFound(nameof(UserProfile), nameof(UserProfile.Auth0Id), auth0Id));

                await redisCache.CacheData(userProfile, TimeSpan.FromMinutes(10), profileKey);
            }

            return mapper.Map<UserProfileModel>(userProfile);
        }

        public async Task<UserProfileModel> UpdateProfileAsync(Guid userId, UserProfileModel userData, string currentUserAuth0Id, CancellationToken cancellationToken = default)
        {
            if (userId != userData.Id)
            {
                throw new BadRequestException(ExceptionMessages.InvalidId(nameof(UserProfileModel), userId));
            }

            var profileKey = nameof(UserProfile) + userId;

            var profile = await redisCache.GetDataFromCache<UserProfile>(profileKey)
                ?? await profileRepository.GetByFilterAsync(p => p.Id == userData.Id, cancellationToken)
                ?? throw new NotFoundException(ExceptionMessages.NotFound(nameof(UserProfile), nameof(UserProfile.Id), userData.Id));

            if (currentUserAuth0Id != profile.Auth0Id)
            {
                throw new ForbiddenException(ExceptionMessages.AccessDenied(nameof(UserProfile), profile.Id));
            }

            var userToUpdate = mapper.Map<UserProfile>(userData);

            profile = await profileRepository.UpdateAsync(userToUpdate, cancellationToken);

            await redisCache.CacheData(profile, TimeSpan.FromMinutes(10), profileKey);
            await redisCache.CacheData(profile, TimeSpan.FromMinutes(10), nameof(UserProfile.Auth0Id) + profile.Auth0Id);

            return mapper.Map<UserProfileModel>(profile);
        }
    }
}
