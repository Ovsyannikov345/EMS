using AutoMapper;
using Grpc.Core;
using CatalogueService.BLL.Grpc.Models;

namespace CatalogueService.BLL.Grpc.Services
{
    public class ProfileGrpcClient(ProfileService.ProfileServiceClient client, IMapper mapper)
    {
        public async Task<UserProfile> GetProfile(Guid id)
        {
            var profileResponse = await client.GetProfileAsync(new ProfileRequest { Id = id.ToString() });

            return mapper.Map<UserProfile>(profileResponse.Profile);
        }

        public async Task<UserProfile> GetOwnProfile(string auth0Id, ServerCallContext context)
        {
            var profileResponse = await client.GetOwnProfileAsync(new OwnProfileRequest { Auth0Id = auth0Id });

            return mapper.Map<UserProfile>(profileResponse.Profile);
        }
    }
}