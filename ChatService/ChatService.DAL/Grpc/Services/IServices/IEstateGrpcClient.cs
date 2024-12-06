using ChatService.DAL.Grpc.Services.Estate;

namespace ChatService.DAL.Grpc.Services.IServices
{
    public interface IEstateGrpcClient
    {
        Task<EstateResponse> GetEstateAsync(Guid estateId, CancellationToken cancellationToken = default);

        Task<EstateListResponse> GetEstateListAsync(IEnumerable<Guid> estateIds, CancellationToken cancellationToken = default);

        Task<EstateListResponse> GetUserEstateAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
