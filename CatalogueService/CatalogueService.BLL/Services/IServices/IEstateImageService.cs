using CatalogueService.BLL.Models;
using Microsoft.AspNetCore.Http;

namespace CatalogueService.BLL.Services.IServices
{
    public interface IEstateImageService
    {
        Task DeleteImageAsync(Guid estateId, string userAuth0Id, Guid imageId, CancellationToken cancellationToken = default);

        Task<Stream> GetImageAsync(Guid estateId, Guid imageId, CancellationToken cancellationToken = default);

        Task<List<string>> GetImageNameListAsync(Guid estateId, CancellationToken cancellationToken = default);

        Task<string> UploadImageAsync(Guid estateId, string userAuth0Id, IFormFile file, CancellationToken cancellationToken = default);
    }
}