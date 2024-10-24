using Moq;
using ProfileService.DAL.Models;
using ProfileService.DAL.Repositories.IRepositories;
using System.Linq.Expressions;

namespace ProfileService.UnitTests.Mocks;

internal class ProfileRepositoryMock : Mock<IProfileRepository>
{
    private readonly CancellationToken _anyToken = It.IsAny<CancellationToken>();

    public void GetByIdAsync(UserProfile userProfileToReturn) =>
        Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), _anyToken))
            .ReturnsAsync(userProfileToReturn);

    public void GetByIdAsyncReturnsNull() =>
        Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), _anyToken))
            .ReturnsAsync((UserProfile?)null);

    public void GetByFilterAsync(UserProfile userProfileToReturn) =>
        Setup(repo => repo.GetByFilterAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), _anyToken))
            .ReturnsAsync(userProfileToReturn);

    public void GetByFilterAsyncReturnsNull() =>
        Setup(repo => repo.GetByFilterAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), _anyToken))
            .ReturnsAsync((UserProfile?)null);

    public void GetAllAsync(IEnumerable<UserProfile> userProfilesToReturn) =>
        Setup(repo => repo.GetAllAsync(_anyToken))
            .ReturnsAsync(userProfilesToReturn);

    public void CreateAsync(UserProfile createdUserProfile) =>
        Setup(repo => repo.CreateAsync(It.IsAny<UserProfile>(), _anyToken))
            .ReturnsAsync(createdUserProfile);

    public void UpdateAsync(UserProfile updatedUserProfile) =>
        Setup(repo => repo.UpdateAsync(It.IsAny<UserProfile>(), _anyToken))
            .ReturnsAsync(updatedUserProfile);

    public void DeleteAsync(UserProfile deletedUserProfile) =>
        Setup(repo => repo.DeleteAsync(It.IsAny<Guid>(), _anyToken))
            .ReturnsAsync(deletedUserProfile);

    public void DeleteAsyncReturnsNull() =>
        Setup(repo => repo.DeleteAsync(It.IsAny<Guid>(), _anyToken))
            .ReturnsAsync((UserProfile?)null);
}

