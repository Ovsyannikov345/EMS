using Moq;
using ProfileService.BLL.Models;
using ProfileService.BLL.Services.IServices;

namespace ProfileService.UnitTests.Mocks
{
    internal class ProfileServiceMock : Mock<IUserProfileService>
    {
        private readonly CancellationToken _anyToken = It.IsAny<CancellationToken>();

        public void GetProfileAsync(UserProfileModelWithPrivacy profileModel) =>
            Setup(s => s.GetProfileAsync(It.IsAny<Guid>(), _anyToken))
                .ReturnsAsync(profileModel);

        public void GetOwnProfileAsync(UserProfileModel profileModel) =>
            Setup(s => s.GetOwnProfileAsync(It.IsAny<string>(), _anyToken))
                .ReturnsAsync(profileModel);
    }
}
