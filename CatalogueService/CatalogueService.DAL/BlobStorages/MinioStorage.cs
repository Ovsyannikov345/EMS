using CatalogueService.DAL.BlobStorages.IBlobStorages;
using Microsoft.Extensions.Configuration;
using Minio.DataModel.Args;
using Minio;

namespace CatalogueService.DAL.BlobStorages
{
    public class MinioStorage : IMinioStorage
    {
        private readonly IMinioClient _minioClient;

        private readonly string _bucketName;

        public MinioStorage(IMinioClient minioClient, IConfiguration configuration)
        {
            _minioClient = minioClient;
            _bucketName = configuration["Minio:BucketName"]!;
        }

        public async Task SaveFileAsync(Stream fileStream, string objectName, string contentType, CancellationToken cancellationToken = default)
        {
            await EnsureBucketCreated(cancellationToken);

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType(contentType);

            await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
        }

        public async Task<Stream?> GetFileAsync(string objectName, CancellationToken cancellationToken = default)
        {
            try
            {
                await _minioClient.StatObjectAsync(
                new StatObjectArgs().WithBucket(_bucketName).WithObject(objectName), cancellationToken);
            }
            catch
            {
                return null;
            }

            var objectStream = new MemoryStream();
            await _minioClient.GetObjectAsync(new GetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithCallbackStream((stream) => stream.CopyTo(objectStream)), cancellationToken);

            objectStream.Position = 0;

            return objectStream;
        }

        public async Task<List<string>> GetFileNamesByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
        {
            await EnsureBucketCreated(cancellationToken);

            var imageKeys = new List<string>();

            await foreach (var item in _minioClient.ListObjectsEnumAsync(
                new ListObjectsArgs().WithBucket(_bucketName).WithPrefix($"{prefix}/"), cancellationToken))
            {
                imageKeys.Add(item.Key.Split("/")[1]);
            }

            return imageKeys;
        }

        public async Task DeleteFileAsync(string objectName, CancellationToken cancellationToken = default)
        {
            await EnsureBucketCreated(cancellationToken);

            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName), cancellationToken);
        }

        private async Task EnsureBucketCreated(CancellationToken cancellationToken = default)
        {
            bool isBucketExists = await _minioClient.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(_bucketName), cancellationToken);

            if (!isBucketExists)
            {
                await _minioClient.MakeBucketAsync(
                    new MakeBucketArgs().WithBucket(_bucketName), cancellationToken);
            }
        }
    }
}
