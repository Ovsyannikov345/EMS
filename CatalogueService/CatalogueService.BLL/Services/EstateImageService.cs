using CatalogueService.BLL.Utilities.Exceptions.Messages;
using CatalogueService.BLL.Utilities.Exceptions;
using CatalogueService.DAL.BlobStorages.IBlobStorages;
using SixLabors.ImageSharp;
using CatalogueService.BLL.Services.IServices;
using CatalogueService.DAL.Repositories.IRepositories;
using CatalogueService.DAL.Models.Entities;
using CatalogueService.DAL.Grpc.Services.IServices;
using Microsoft.AspNetCore.Http;

namespace CatalogueService.BLL.Services
{
    public class EstateImageService(
        IEstateRepository estateRepository,
        IProfileGrpcClient profileGrpcClient,
        IMinioStorage minioStorage) : IEstateImageService
    {
        private static readonly int _maxFileSizeInBytes = 2 * 1024 * 1024;

        private static readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png"];

        private static readonly int _maxImagesPerEstate = 5;

        public async Task<Stream> GetImageAsync(Guid estateId, Guid imageId, CancellationToken cancellationToken = default)
        {
            var objectFullName = $"{estateId}/{imageId}";

            var stream = await minioStorage.GetFileAsync(objectFullName, cancellationToken)
                ?? throw new NotFoundException(FileExceptionMessages.NotFound);

            return stream;
        }

        public async Task UploadImageAsync(Guid estateId, string userAuth0Id, IFormFile file, CancellationToken cancellationToken = default)
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

            var estate = await estateRepository.GetByIdAsync(estateId, cancellationToken)
                ?? throw new NotFoundException(
                    ExceptionMessages.NotFound(nameof(Estate), nameof(Estate.Id), estateId));

            var profile = await profileGrpcClient.GetOwnProfile(userAuth0Id, cancellationToken);

            if (estate.UserId != profile.Id)
            {
                throw new ForbiddenException(ExceptionMessages.AccessDenied(nameof(Estate), estateId));
            }

            var imageNames = await GetImageNameListAsync(estateId, cancellationToken);

            if (imageNames.Count >= _maxImagesPerEstate)
            {
                throw new BadRequestException(FileExceptionMessages.CountLimitExceeded(_maxImagesPerEstate));
            }

            var newImageName = $"{estateId}/{Guid.NewGuid()}";

            using var fileStream = file.OpenReadStream();

            using var jpegStream = await ConvertToJpegStream(fileStream, cancellationToken);

            await minioStorage.SaveFileAsync(jpegStream, newImageName, "image/jpeg", cancellationToken);
        }

        public async Task<List<string>> GetImageNameListAsync(Guid estateId, CancellationToken cancellationToken = default)
        {
            return await minioStorage.GetFileNamesByPrefixAsync(estateId.ToString(), cancellationToken);
        }

        public async Task DeleteImageAsync(Guid estateId, string userAuth0Id, Guid imageId, CancellationToken cancellationToken = default)
        {
            var estate = await estateRepository.GetByIdAsync(estateId, cancellationToken)
                ?? throw new NotFoundException(
                    ExceptionMessages.NotFound(nameof(Estate), nameof(Estate.Id), estateId));

            var profile = await profileGrpcClient.GetOwnProfile(userAuth0Id, cancellationToken);

            if (estate.UserId != profile.Id)
            {
                throw new ForbiddenException(ExceptionMessages.AccessDenied(nameof(Estate), estateId));
            }

            var imageNames = await GetImageNameListAsync(estateId, cancellationToken);

            if (!imageNames.Contains(imageId.ToString()))
            {
                throw new NotFoundException(FileExceptionMessages.NotFound);
            }

            var objectFullName = $"{estateId}/{imageId}";

            await minioStorage.DeleteFileAsync(objectFullName, cancellationToken);
        }

        private static async Task<MemoryStream> ConvertToJpegStream(Stream fileStream, CancellationToken cancellationToken = default)
        {
            var jpegStream = new MemoryStream();

            using (var image = await Image.LoadAsync(fileStream, cancellationToken))
            {
                await image.SaveAsJpegAsync(jpegStream, cancellationToken);
            }

            jpegStream.Position = 0;

            return jpegStream;
        }
    }
}
