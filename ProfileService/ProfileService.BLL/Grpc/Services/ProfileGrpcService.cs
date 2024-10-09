using AutoMapper;
using Grpc.Core;
using ProfileService.BLL.Services.IServices;
using ProfileService.BLL.Utilities.Exceptions;
using ProfileService.BLL.Utilities.Messages;

namespace ProfileService.BLL.Grpc.Services
{
    public class ProfileGrpcService(IUserProfileService userProfileService, IMapper mapper) : ProfileService.ProfileServiceBase
    {
        public override async Task<ProfileResponse> GetProfile(ProfileRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.Id, out Guid profileId))
            {
                throw new BadRequestException(UserProfileMessages.InvalidId);
            }

            var profile = await userProfileService.GetProfileAsync(profileId, context.CancellationToken);

            return new ProfileResponse
            {
                Profile = mapper.Map<ProtoProfileModel>(profile)
            };
        }

        public override async Task<ProfileResponse> GetOwnProfile(OwnProfileRequest request, ServerCallContext context)
        {
            var profile = await userProfileService.GetOwnProfileAsync(request.Auth0Id, context.CancellationToken);

            return new ProfileResponse
            {
                Profile = mapper.Map<ProtoProfileModel>(profile)
            };
        }
    }
}
