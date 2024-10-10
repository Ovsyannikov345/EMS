using AutoMapper;
using CatalogueService.BLL.Grpc.Models;
using CatalogueService.BLL.Grpc.Services.IServices;

namespace CatalogueService.BLL.Grpc.Services
{
    public class ProfileGrpcClient(ProfileService.ProfileServiceClient client, IMapper mapper) : IProfileGrpcClient
    {
        public async Task<UserProfile> GetProfile(Guid id, CancellationToken cancellationToken = default)
        {
            var profileResponse = await client.GetProfileAsync(new ProfileRequest { Id = id.ToString() }, cancellationToken: cancellationToken);

            return mapper.Map<UserProfile>(profileResponse.Profile);
        }

        public async Task<UserProfile> GetOwnProfile(string auth0Id, CancellationToken cancellationToken = default)
        {
            var profileResponse = await client.GetOwnProfileAsync(new OwnProfileRequest { Auth0Id = auth0Id }, cancellationToken: cancellationToken);

            return mapper.Map<UserProfile>(profileResponse.Profile);
        }
    }
}