using ChatService.DAL.Grpc.Services.Estate;
using ChatService.DAL.Grpc.Services.IServices;
namespace ChatService.DAL.Grpc.Services
{
    public class EstateGrpcClient(EstateGrpcServiceProto.EstateGrpcServiceProtoClient client) : IEstateGrpcClient
    {
        public async Task<EstateResponse> GetEstateAsync(Guid estateId, CancellationToken cancellationToken = default)
        {
            var response = await client.GetEstateAsync(new EstateRequest { Id = estateId.ToString() }, cancellationToken: cancellationToken);

            return response;
        }

        public async Task<EstateListResponse> GetEstateListAsync(IEnumerable<Guid> estateIds, CancellationToken cancellationToken = default)
        {
            var request = new EstateListRequest();

            request.EstateIds.AddRange(estateIds.Select(id => id.ToString()));

            var response = await client.GetEstateListAsync(request, cancellationToken: cancellationToken);

            return response;
        }

        public async Task<EstateListResponse> GetUserEstateAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var response = await client.GetUserEstateAsync(
                new UserEstateRequest { UserId = userId.ToString() }, cancellationToken: cancellationToken);

            return response;
        }
    }
}
