using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileService.BLL.Models;
using ProfileService.BLL.Services.IServices;
using ProfileService.BLL.Utilities.Exceptions;
using ProfileService.BLL.Utilities.Messages;
using ProfileService.ViewModels;
using System.Security.Claims;

namespace ProfileService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController(IUserProfileService profileService, IProfileInfoVisibilityService visibilityService, IMapper mapper) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<UserProfileViewModel> CreateProfile(RegistrationDataViewModel userData, CancellationToken cancellationToken)
        {
            var profile = await profileService.CreateProfileAsync(mapper.Map<RegistrationDataModel>(userData), cancellationToken);

            return mapper.Map<UserProfileViewModel>(profile);
        }

        [HttpGet("{id}")]
        public async Task<UserProfileViewModel> GetUserProfile(Guid id, CancellationToken cancellationToken)
        {
            var profile = await profileService.GetProfileAsync(id, cancellationToken);

            return mapper.Map<UserProfileViewModel>(profile);
        }

        [HttpGet("my")]
        public async Task<UserProfileViewModel> GetOwnProfile(CancellationToken cancellationToken)
        {
            var auth0Id = GetAuth0IdFromContext();

            var profile = await profileService.GetOwnProfileAsync(auth0Id, cancellationToken);

            return mapper.Map<UserProfileViewModel>(profile);
        }

        [HttpGet("{userId}/visibility")]
        public async Task<ProfileInfoVisibilityViewModel> GetVisibilityOptions(Guid userId, CancellationToken cancellationToken)
        {
            var visibility = await visibilityService.GetProfileInfoVisibilityAsync(userId, cancellationToken);

            return mapper.Map<ProfileInfoVisibilityViewModel>(visibility);
        }

        [HttpPut("{id}")]
        public async Task<UserProfileViewModel> UpdateProfile(Guid id, UserProfileViewModel userData, CancellationToken cancellationToken)
        {
            if (id != userData.Id)
            {
                throw new BadRequestException(UserProfileMessages.InvalidId);
            }

            var auth0Id = GetAuth0IdFromContext();

            var updatedProfile = await profileService.UpdateProfileAsync(mapper.Map<UserProfileModel>(userData), auth0Id, cancellationToken);

            return mapper.Map<UserProfileViewModel>(updatedProfile);
        }

        [HttpPut("{userId}/visibility")]
        public async Task<ProfileInfoVisibilityViewModel> UpdateVisibilityOptions(Guid userId, ProfileInfoVisibilityViewModel visibilityData, CancellationToken cancellationToken)
        {
            if (userId != visibilityData.UserId)
            {
                throw new BadRequestException(ProfileVisibilityMessages.InvalidId);
            }

            var auth0Id = GetAuth0IdFromContext();

            var visibilityToUpdate = mapper.Map<ProfileInfoVisibilityModel>(visibilityData);

            var updatedVisibility = await visibilityService.UpdateProfileInfoVisibilityAsync(auth0Id, visibilityToUpdate, cancellationToken);

            return mapper.Map<ProfileInfoVisibilityViewModel>(updatedVisibility);
        }

        private string GetAuth0IdFromContext() => HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
    }
}
