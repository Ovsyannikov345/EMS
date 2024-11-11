using Moq;
using ProfileService.DAL.CacheRepositoryManagers.ICacheRepositoryManagers;
using ProfileService.DAL.Models.Interfaces;
using System.Linq.Expressions;

namespace ProfileService.UnitTests.Mocks
{
    public class CacheRepositoryManagerMock<T> : Mock<ICacheRepositoryManager<T>> where T : class, ICacheable
    {
        private readonly CancellationToken _anyToken = It.IsAny<CancellationToken>();

        public void GetEntityByIdAsync(T entityToReturn) =>
            Setup(mng => mng.GetEntityByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), _anyToken))
                .ReturnsAsync(entityToReturn);

        public void GetEntityByIdAsyncReturnsNull() =>
            Setup(mng => mng.GetEntityByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), _anyToken))
                .ReturnsAsync((T?)null);

        public void GetEntityByFilterAsync(T entityToReturn) =>
            Setup(mng => mng.GetEntityByFilterAsync(It.IsAny<string>(), It.IsAny<Expression<Func<T, bool>>>(), It.IsAny<bool>(), _anyToken))
                .ReturnsAsync(entityToReturn);

        public void GetEntityByFilterAsyncReturnsNull() =>
            Setup(mng => mng.GetEntityByFilterAsync(It.IsAny<string>(), It.IsAny<Expression<Func<T, bool>>>(), It.IsAny<bool>(), _anyToken))
                .ReturnsAsync((T?)null);

        public void UpdateEntityAsync(T updatedEntity) =>
            Setup(mng => mng.UpdateEntityAsync(It.IsAny<T>(), It.IsAny<string[]>(), _anyToken))
                .ReturnsAsync(updatedEntity);
    }
}
