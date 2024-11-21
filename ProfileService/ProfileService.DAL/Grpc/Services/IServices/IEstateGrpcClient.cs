namespace ProfileService.DAL.Grpc.Services.IServices
{
    public interface IEstateGrpcClient
    {
        Task<int> GetEstateCount(Guid userId, CancellationToken cancellationToken = default);
    }
}