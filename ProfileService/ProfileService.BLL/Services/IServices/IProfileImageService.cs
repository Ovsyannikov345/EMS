using Microsoft.AspNetCore.Http;

namespace ProfileService.BLL.Services.IServices
{
    public interface IProfileImageService
    {
        Task UploadProfileImageAsync(string auth0Id, IFormFile file, CancellationToken cancellationToken = default);

        Task<Stream> GetProfileImageAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<Stream> GetProfileImageAsync(string auth0Id, CancellationToken cancellationToken = default);
    }
}