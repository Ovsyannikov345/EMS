﻿using ProfileService.BLL.Dto;
using ProfileService.BLL.Models;
using ProfileService.DAL.Models;

namespace ProfileService.BLL.Services.IServices
{
    public interface IUserProfileService
    {
        Task<UserProfile> CreateProfileAsync(UserRegistrationData userData, CancellationToken cancellationToken = default);

        Task<UserProfile> GetProfileAsync(Guid id, CancellationToken cancellationToken = default);

        Task<UserProfile> GetOwnProfileAsync(string auth0Id, CancellationToken cancellationToken = default);

        Task<UserProfileModel> UpdateProfileAsync(UserProfileModel userData, string currentUserAuth0Id, CancellationToken cancellationToken = default);
    }
}
