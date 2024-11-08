namespace ProfileService.DAL.CacheProvider.ICacheProvider
{
    public interface IRedisCacheProvider
    {
        Task CacheData<T>(T data, TimeSpan cacheLifetime, string key);

        Task<T?> GetDataFromCache<T>(string key);
    }
}