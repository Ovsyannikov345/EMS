using ProfileService.BLL.Dto;
using ProfileService.DAL.Models;

namespace ProfileService.BLL.Services.IServices
{
    public interface IUserProfileService
    {
        public Task<UserProfile> CreateProfileAsync(UserRegistrationData userData, CancellationToken cancellationToken = default);

        public Task<UserProfile> GetProfileAsync(Guid id, CancellationToken cancellationToken = default);

        public Task<UserProfile> GetOwnProfileAsync(string auth0Id, CancellationToken cancellationToken = default);
    }
}
