using CatalogueService.BLL.Grpc.Services;

namespace ChatService.DAL.Grpc.Services.IServices
{
    public interface IEstateGrpcClient
    {
        public Task<EstateResponse> GetEstateAsync(Guid estateId, CancellationToken cancellationToken = default);
    }
}
