using ChatService.DAL.Grpc.Services.Profile;

namespace ChatService.DAL.Grpc.Services.IServices
{
    public interface IProfileGrpcClient
    {
        Task<ProfileResponse> GetProfile(Guid id, CancellationToken cancellationToken = default);

        Task<ProfileResponse> GetOwnProfile(string auth0Id, CancellationToken cancellationToken = default);
    }
}
