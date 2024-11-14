using CatalogueService.BLL.Services.IServices;
using CatalogueService.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogueService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EstateImageController(IEstateImageService estateImageService) : ControllerBase
    {
        [HttpPost("{estateId}")]
        public async Task UploadImage(Guid estateId, IFormFile file, CancellationToken cancellationToken)
        {
            var auth0Id = HttpContext.GetAuth0IdFromContext();

            await estateImageService.UploadImageAsync(estateId, auth0Id, file, cancellationToken);
        }

        [HttpGet("{estateId}")]
        public async Task<List<string>> GetImageNameList(Guid estateId, CancellationToken cancellationToken)
        {
            return await estateImageService.GetImageNameListAsync(estateId, cancellationToken);
        }

        [HttpGet("{estateId}/{imageId}")]
        public async Task<FileStreamResult> GetEstateImage(Guid estateId, Guid imageId, CancellationToken cancellationToken)
        {
            var stream = await estateImageService.GetImageAsync(estateId, imageId, cancellationToken);

            return File(stream, "image/png");
        }

        [HttpDelete("{estateId}/{imageId}")]
        public async Task DeleteEstateImage(Guid estateId, Guid imageId, CancellationToken cancellationToken)
        {
            var auth0Id = HttpContext.GetAuth0IdFromContext();

            await estateImageService.DeleteImageAsync(estateId, auth0Id, imageId, cancellationToken);
        }
    }
}
