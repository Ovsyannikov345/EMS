using CatalogueService.BLL.Grpc.Models;

namespace CatalogueService.BLL.Grpc.Services.IServices
{
    public interface IProfileGrpcClient
    {
        public Task<UserProfile> GetProfile(Guid id, CancellationToken cancellationToken = default);

        public Task<UserProfile> GetOwnProfile(string auth0Id, CancellationToken cancellationToken = default);
    }
}
