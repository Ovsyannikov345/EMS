using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;
using ProfileService.DAL.BlobStorages.IBlobStorages;

namespace ProfileService.DAL.BlobStorages
{
    public class MinioStorage(IMinioClient minioClient, IConfiguration configuration) : IMinioStorage
    {
        public async Task SaveImageAsync(string imageId, Stream fileStream, string fileExtension, CancellationToken cancellationToken = default)
        {
            var objectName = $"{imageId}{fileExtension}";

            var bucketName = configuration["Minio:BucketName"];

            var contentType = $"image/{fileExtension[1..]}";

            bool isBucketExists = await minioClient.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(bucketName), cancellationToken);

            if (!isBucketExists)
            {
                await minioClient.MakeBucketAsync(
                    new MakeBucketArgs().WithBucket(bucketName), cancellationToken);
            }

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType(contentType);

            await minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
        }

        public async Task<Stream?> GetImageAsync(string imageId, string fileExtension, CancellationToken cancellationToken = default)
        {
            var objectName = $"{imageId}{fileExtension}";

            var bucketName = configuration["Minio:BucketName"];

            try
            {
                await minioClient.StatObjectAsync(
                new StatObjectArgs().WithBucket(bucketName).WithObject(objectName), cancellationToken);
            }
            catch
            {
                return null;
            }

            var objectStream = new MemoryStream();
            await minioClient.GetObjectAsync(new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithCallbackStream((stream) => stream.CopyTo(objectStream)), cancellationToken);

            objectStream.Position = 0;

            return objectStream;
        }
    }
}
