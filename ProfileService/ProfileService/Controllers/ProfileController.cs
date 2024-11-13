using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileService.BLL.Models;
using ProfileService.BLL.Services.IServices;
using ProfileService.Extensions;
using ProfileService.ViewModels;

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

            var profileViewModel = mapper.Map<UserProfileViewModel>(profile);

            return profileViewModel;
        }

        [HttpGet("my")]
        public async Task<UserProfileViewModel> GetOwnProfile(CancellationToken cancellationToken)
        {
            var auth0Id = HttpContext.GetAuth0IdFromContext();

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
            var auth0Id = HttpContext.GetAuth0IdFromContext();

            var updatedProfile = await profileService.UpdateProfileAsync(id, mapper.Map<UserProfileModel>(userData), auth0Id, cancellationToken);

            return mapper.Map<UserProfileViewModel>(updatedProfile);
        }

        [HttpPut("{userId}/visibility")]
        public async Task<ProfileInfoVisibilityViewModel> UpdateVisibilityOptions(Guid userId, ProfileInfoVisibilityViewModel visibilityData, CancellationToken cancellationToken)
        {
            var auth0Id = HttpContext.GetAuth0IdFromContext();

            var visibilityToUpdate = mapper.Map<ProfileInfoVisibilityModel>(visibilityData);

            var updatedVisibility = await visibilityService.UpdateProfileInfoVisibilityAsync(auth0Id, userId, visibilityToUpdate, cancellationToken);

            return mapper.Map<ProfileInfoVisibilityViewModel>(updatedVisibility);
        }
    }
}
