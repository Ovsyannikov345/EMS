using CatalogueService.DAL.Grpc.Models;

namespace CatalogueService.DAL.Grpc.Services.IServices
{
    public interface IProfileGrpcClient
    {
        Task<UserProfile> GetProfile(Guid id, CancellationToken cancellationToken = default);

        Task<UserProfile> GetOwnProfile(string auth0Id, CancellationToken cancellationToken = default);
    }
}
