using NotificationService.DAL.Grpc.Services.Profile;

namespace NotificationService.DAL.Grpc.Services.IServices
{
    public interface IProfileGrpcClient
    {
        Task<ProfileResponse> GetProfile(Guid id, CancellationToken cancellationToken = default);

        Task<ProfileResponse> GetOwnProfile(string auth0Id, CancellationToken cancellationToken = default);
    }
}
