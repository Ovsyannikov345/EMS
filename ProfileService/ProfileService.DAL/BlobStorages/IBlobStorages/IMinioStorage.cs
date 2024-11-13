namespace ProfileService.DAL.BlobStorages.IBlobStorages
{
    public interface IMinioStorage
    {
        Task SaveImageAsync(string imageId, Stream fileStream, string fileExtension, CancellationToken cancellationToken = default);

        Task<Stream?> GetImageAsync(string imageId, string fileExtension, CancellationToken cancellationToken = default);
    }
}