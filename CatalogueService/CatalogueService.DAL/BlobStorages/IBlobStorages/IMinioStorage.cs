namespace CatalogueService.DAL.BlobStorages.IBlobStorages
{
    public interface IMinioStorage
    {
        Task SaveFileAsync(Stream fileStream, string objectName, string contentType, CancellationToken cancellationToken = default);

        Task<Stream?> GetFileAsync(string objectName, CancellationToken cancellationToken = default);

        Task<List<string>> GetFileNamesByPrefixAsync(string prefix, CancellationToken cancellationToken = default);

        Task DeleteFileAsync(string objectName, CancellationToken cancellationToken = default);
    }
}
