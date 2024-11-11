using ProfileService.DAL.CacheProviders.ICacheProviders;
using ProfileService.DAL.CacheRepositoryManagers.ICacheRepositoryManagers;
using ProfileService.DAL.Models.Interfaces;
using ProfileService.DAL.Repositories.IRepositories;
using Serilog;
using System.Linq.Expressions;

namespace ProfileService.DAL.CacheRepositoryManagers
{
    public class CacheRepositoryManager<T>(ICacheProvider cacheProvider, IGenericRepository<T> repository, ILogger logger) : ICacheRepositoryManager<T> where T : class, ICacheable
    {
        public async Task<T?> GetEntityByIdAsync(Guid entityId, bool updateCache = true, CancellationToken cancellationToken = default)
        {
            var entityKey = typeof(T).Name + entityId;

            var loadedEntity = await cacheProvider.GetDataFromCache<T>(entityKey);

            if (loadedEntity is not null)
            {
                logger.Information("Read from cache");
            }

            if (loadedEntity is null)
            {
                loadedEntity = await repository.GetByFilterAsync(p => p.Id == entityId, cancellationToken);

                if (loadedEntity is not null && updateCache)
                {
                    await cacheProvider.CacheData(loadedEntity, TimeSpan.FromMinutes(10), entityKey);
                }
            }

            return loadedEntity;
        }

        public async Task<T?> GetEntityByFilterAsync(string entityKey, Expression<Func<T, bool>> filter, bool updateCache = true, CancellationToken cancellationToken = default)
        {
            var loadedEntity = await cacheProvider.GetDataFromCache<T>(entityKey);

            if (loadedEntity is not null)
            {
                logger.Information("Read from cache");
            }

            if (loadedEntity is null)
            {
                loadedEntity = await repository.GetByFilterAsync(filter, cancellationToken);

                if (loadedEntity is not null && updateCache)
                {
                    await cacheProvider.CacheData(loadedEntity, TimeSpan.FromMinutes(10), entityKey);
                }
            }

            return loadedEntity;
        }

        public async Task<T?> UpdateEntityAsync(T entityToUdate, string[] entityKeys, CancellationToken cancellationToken = default)
        {
            var updatedEntity = await repository.UpdateAsync(entityToUdate, cancellationToken);

            foreach (var entityKey in entityKeys)
            {
                await cacheProvider.CacheData(updatedEntity, TimeSpan.FromMinutes(10), entityKey);
            }

            return updatedEntity;
        }
    }
}
