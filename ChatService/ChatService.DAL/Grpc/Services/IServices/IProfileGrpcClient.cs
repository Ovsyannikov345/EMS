namespace ChatService.DAL.Grpc.Services.IServices
{
    public interface IProfileGrpcClient
    {
        public Task<ProfileResponse> GetProfile(Guid id, CancellationToken cancellationToken = default);

        public Task<ProfileResponse> GetOwnProfile(string auth0Id, CancellationToken cancellationToken = default);
    }
}
