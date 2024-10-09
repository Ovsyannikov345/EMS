using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileService.BLL.Dto;
using ProfileService.BLL.Services.IServices;
using ProfileService.DAL.Models;
using System.Security.Claims;

namespace ProfileService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController(IUserProfileService profileService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<UserProfile> CreateProfile(UserRegistrationData userData, CancellationToken cancellationToken)
        {
            var profile = await profileService.CreateProfileAsync(userData, cancellationToken);

            return profile;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<UserProfile> GetUserProfile(Guid id, CancellationToken cancellationToken)
        {
            return await profileService.GetProfileAsync(id, cancellationToken);
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<UserProfile> GetOwnProfile(CancellationToken cancellationToken)
        {
            var auth0Id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            return await profileService.GetOwnProfileAsync(auth0Id, cancellationToken);
        }
    }
}
