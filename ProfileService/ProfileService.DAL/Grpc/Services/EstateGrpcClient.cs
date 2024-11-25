using ProfileService.DAL.Grpc.Services.IServices;

namespace ProfileService.DAL.Grpc.Services
{
    public class EstateGrpcClient(EstateGrpcServiceProto.EstateGrpcServiceProtoClient client) : IEstateGrpcClient
    {
        public async Task<int> GetEstateCount(Guid userId, CancellationToken cancellationToken = default)
        {
            var response = await client.GetUserEstateCountAsync(new EstateCountRequest
            {
                UserId = userId.ToString(),
            }, cancellationToken: cancellationToken);

            return response.Count;
        }
    }
}
