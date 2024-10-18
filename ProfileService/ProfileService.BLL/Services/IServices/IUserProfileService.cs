using ProfileService.BLL.Models;

namespace ProfileService.BLL.Services.IServices
{
    public interface IUserProfileService
    {
        Task<UserProfileModel> CreateProfileAsync(RegistrationDataModel userData, CancellationToken cancellationToken = default);

        Task<UserProfileModelWithPrivacy> GetProfileAsync(Guid id, CancellationToken cancellationToken = default);

        Task<UserProfileModel> GetOwnProfileAsync(string auth0Id, CancellationToken cancellationToken = default);

        Task<UserProfileModel> UpdateProfileAsync(Guid userId, UserProfileModel userData, string currentUserAuth0Id, CancellationToken cancellationToken = default);
    }
}
