using ProfileService.DAL.CacheProviders.ICacheProviders;
using StackExchange.Redis;
using System.Text.Json;

namespace ProfileService.DAL.CacheProviders
{
    public class RedisCacheProvider : ICacheProvider
    {
        private readonly IDatabase _redis;

        public RedisCacheProvider(IConnectionMultiplexer connectionMultiplexer)
        {
            _redis = connectionMultiplexer.GetDatabase();
        }

        public async Task<T?> GetDataFromCache<T>(string key)
        {
            var json = await _redis.StringGetAsync(key);

            if (string.IsNullOrEmpty(json))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(json!);
        }

        public async Task CacheData<T>(T data, TimeSpan cacheLifetime, string key)
        {
            var json = JsonSerializer.Serialize(data);

            var setTask = _redis.StringSetAsync(key, json);

            var expireTask = _redis.KeyExpireAsync(key, cacheLifetime);

            await Task.WhenAll(setTask, expireTask);
        }
    }
}
