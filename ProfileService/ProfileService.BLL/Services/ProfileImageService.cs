using Microsoft.AspNetCore.Http;
using ProfileService.BLL.Services.IServices;
using ProfileService.BLL.Utilities.Exceptions;
using ProfileService.BLL.Utilities.Exceptions.Messages;
using ProfileService.DAL.BlobStorages.IBlobStorages;
using ProfileService.DAL.CacheRepositoryManagers.ICacheRepositoryManagers;
using ProfileService.DAL.Models;
using SixLabors.ImageSharp;

namespace ProfileService.BLL.Services
{
    public class ProfileImageService(
        ICacheRepositoryManager<UserProfile> profileCacheRepositoryManager,
        IMinioStorage minioStorage) : IProfileImageService
    {
        private static readonly int _maxFileSizeInBytes = 2 * 1024 * 1024;

        private static readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png"];

        public async Task UploadProfileImageAsync(string auth0Id, IFormFile file, CancellationToken cancellationToken = default)
        {
            if (file == null || file.Length == 0)
            {
                throw new BadRequestException(FileExceptionMessages.EmptyFile);
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!_allowedExtensions.Contains(fileExtension))
            {
                throw new BadRequestException(FileExceptionMessages.InvalidFormat);
            }

            if (file.Length > _maxFileSizeInBytes)
            {
                throw new BadRequestException(FileExceptionMessages.FileSizeExceeded);
            }

            var profile = await profileCacheRepositoryManager.GetEntityByFilterAsync(
                nameof(UserProfile.Auth0Id) + auth0Id, u => u.Auth0Id == auth0Id, cancellationToken: cancellationToken)
                ?? throw new NotFoundException(ExceptionMessages.NotFound(nameof(UserProfile), nameof(UserProfile.Auth0Id), auth0Id));

            using var stream = file.OpenReadStream();

            var jpegStream = await ConvertToJpegStream(stream, cancellationToken);

            await minioStorage.SaveImageAsync(profile.Id.ToString(), jpegStream, ".jpeg", cancellationToken);
        }

        public async Task<Stream> GetProfileImageAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var profile = await profileCacheRepositoryManager.GetEntityByIdAsync(
                userId, cancellationToken: cancellationToken)
                ?? throw new NotFoundException(ExceptionMessages.NotFound(nameof(UserProfile), nameof(UserProfile.Id), userId));

            var stream = await minioStorage.GetImageAsync(profile.Id.ToString(), ".jpeg", cancellationToken)
                ?? throw new NotFoundException(FileExceptionMessages.NotFound);

            return stream;
        }

        public async Task<Stream> GetProfileImageAsync(string auth0Id, CancellationToken cancellationToken = default)
        {
            var profile = await profileCacheRepositoryManager.GetEntityByFilterAsync(
                nameof(UserProfile.Auth0Id) + auth0Id, u => u.Auth0Id == auth0Id, cancellationToken: cancellationToken)
                ?? throw new NotFoundException(ExceptionMessages.NotFound(nameof(UserProfile), nameof(UserProfile.Auth0Id), auth0Id));

            var stream = await minioStorage.GetImageAsync(profile.Id.ToString(), ".jpeg", cancellationToken)
                ?? throw new NotFoundException(FileExceptionMessages.NotFound);

            return stream;
        }

        private static async Task<MemoryStream> ConvertToJpegStream(Stream fileStream, CancellationToken cancellationToken = default)
        {
            var jpegStream = new MemoryStream();

            using (var image = await Image.LoadAsync(fileStream, cancellationToken))
            {
                image.SaveAsJpeg(jpegStream);
            }

            jpegStream.Position = 0;

            return jpegStream;
        }
    }
}
