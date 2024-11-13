using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileService.BLL.Services.IServices;
using ProfileService.Extensions;

namespace ProfileService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileImageController(IProfileImageService profileImageService) : ControllerBase
    {
        [HttpPost]
        public async Task UploadProfileImage(IFormFile file, CancellationToken cancellationToken)
        {
            await profileImageService.UploadProfileImageAsync(HttpContext.GetAuth0IdFromContext(), file, cancellationToken);
        }

        [HttpGet("my")]
        public async Task<FileStreamResult> GetOwnProfileImage(CancellationToken cancellationToken)
        {
            var stream = await profileImageService.GetProfileImageAsync(HttpContext.GetAuth0IdFromContext(), cancellationToken);

            return File(stream, "image/jpeg");
        }

        [HttpGet("{userId}")]
        public async Task<FileStreamResult> GetProfileImage(Guid userId, CancellationToken cancellationToken)
        {
            var stream = await profileImageService.GetProfileImageAsync(userId, cancellationToken);

            return File(stream, "image/jpeg");
        }
    }
}
