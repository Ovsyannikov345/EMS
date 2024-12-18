﻿using ChatService.DAL.Grpc.Services.IServices;
using ChatService.DAL.Grpc.Services.Profile;

namespace ChatService.DAL.Grpc.Services
{
    public class ProfileGrpcClient(ProfileService.ProfileServiceClient client) : IProfileGrpcClient
    {
        public async Task<ProfileResponse> GetProfile(Guid id, CancellationToken cancellationToken = default)
        {
            return await client.GetProfileAsync(new ProfileRequest { Id = id.ToString() }, cancellationToken: cancellationToken);
        }

        public async Task<ProfileResponse> GetOwnProfile(string auth0Id, CancellationToken cancellationToken = default)
        {
            return await client.GetOwnProfileAsync(new OwnProfileRequest { Auth0Id = auth0Id }, cancellationToken: cancellationToken);
        }
    }
}
