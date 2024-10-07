using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileService.BLL.Dto;
using ProfileService.BLL.Services.IServices;

namespace ProfileService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController(IUserProfileService profileService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateProfile(UserRegistrationData userData, CancellationToken cancellationToken)
        {
            var profile = await profileService.CreateProfileAsync(userData, cancellationToken);

            return Ok(profile);
        }
    }
}
