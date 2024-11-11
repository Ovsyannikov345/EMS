using ProfileService.DAL.Models.Interfaces;
using System.Linq.Expressions;

namespace ProfileService.DAL.CacheRepositoryManagers.ICacheRepositoryManagers
{
    public interface ICacheRepositoryManager<T> where T : class, ICacheable
    {
        Task<T?> GetEntityByIdAsync(Guid entityId, bool updateCache = true, CancellationToken cancellationToken = default);

        Task<T?> GetEntityByFilterAsync(string entityKey, Expression<Func<T, bool>> filter, bool updateCache = true, CancellationToken cancellationToken = default);

        Task<T?> UpdateEntityAsync(T entityToUdate, string[] entityKeys, CancellationToken cancellationToken = default);
    }
}