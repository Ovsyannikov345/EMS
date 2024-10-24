using Moq;
using ProfileService.DAL.Models;
using ProfileService.DAL.Repositories.IRepositories;
using System.Linq.Expressions;

namespace ProfileService.UnitTests.Mocks
{
    internal class ProfileInfoVisibilityRepositoryMock : Mock<IProfileInfoVisibilityRepository>
    {
        private readonly CancellationToken _anyToken = It.IsAny<CancellationToken>();

        public void GetByIdAsync(ProfileInfoVisibility profileInfoVisibilityToReturn) =>
            Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), _anyToken))
                .ReturnsAsync(profileInfoVisibilityToReturn);

        public void GetByIdAsyncReturnsNull() =>
            Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), _anyToken))
                .ReturnsAsync((ProfileInfoVisibility?)null);

        public void GetByFilterAsync(ProfileInfoVisibility profileInfoVisibilityToReturn) =>
            Setup(repo => repo.GetByFilterAsync(It.IsAny<Expression<Func<ProfileInfoVisibility, bool>>>(), _anyToken))
                .ReturnsAsync(profileInfoVisibilityToReturn);

        public void GetByFilterAsyncReturnsNull() =>
            Setup(repo => repo.GetByFilterAsync(It.IsAny<Expression<Func<ProfileInfoVisibility, bool>>>(), _anyToken))
                .ReturnsAsync((ProfileInfoVisibility?)null);

        public void GetAllAsync(IEnumerable<ProfileInfoVisibility> profileInfoVisibilitiesToReturn) =>
            Setup(repo => repo.GetAllAsync(_anyToken))
                .ReturnsAsync(profileInfoVisibilitiesToReturn);

        public void CreateAsync(ProfileInfoVisibility createdProfileInfoVisibility) =>
            Setup(repo => repo.CreateAsync(It.IsAny<ProfileInfoVisibility>(), _anyToken))
                .ReturnsAsync(createdProfileInfoVisibility);

        public void UpdateAsync(ProfileInfoVisibility updatedProfileInfoVisibility) =>
            Setup(repo => repo.UpdateAsync(It.IsAny<ProfileInfoVisibility>(), _anyToken))
                .ReturnsAsync(updatedProfileInfoVisibility);

        public void DeleteAsync(ProfileInfoVisibility deletedProfileInfoVisibility) =>
            Setup(repo => repo.DeleteAsync(It.IsAny<Guid>(), _anyToken))
                .ReturnsAsync(deletedProfileInfoVisibility);

        public void DeleteAsyncReturnsNull() =>
            Setup(repo => repo.DeleteAsync(It.IsAny<Guid>(), _anyToken))
                .ReturnsAsync((ProfileInfoVisibility?)null);
    }
}
