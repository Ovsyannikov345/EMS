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
    }
}
