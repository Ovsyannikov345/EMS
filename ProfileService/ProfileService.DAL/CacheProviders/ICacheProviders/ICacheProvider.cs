namespace ProfileService.DAL.CacheProviders.ICacheProviders
{
    public interface ICacheProvider
    {
        Task CacheData<T>(T data, TimeSpan cacheLifetime, string key);

        Task<T?> GetDataFromCache<T>(string key);
    }
}