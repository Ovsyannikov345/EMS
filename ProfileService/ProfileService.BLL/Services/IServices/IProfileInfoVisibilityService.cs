using ProfileService.BLL.Models;

namespace ProfileService.BLL.Services.IServices
{
    public interface IProfileInfoVisibilityService
    {
        Task<ProfileInfoVisibilityModel> GetProfileInfoVisibilityAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<ProfileInfoVisibilityModel> UpdateProfileInfoVisibilityAsync(string currentUserAuth0Id, ProfileInfoVisibilityModel visibilityData, CancellationToken cancellationToken = default);
    }
}
