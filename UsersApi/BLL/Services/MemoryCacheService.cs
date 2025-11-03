using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using UsersApi.Abstractions;
using UsersApi.Properties;

namespace UsersApi.BLL.Services
{
    public class MemoryCacheService(IMemoryCache cache,
        IOptions<AppSettingsConfig> settingsConfig) : IMemoryCacheService
    {
        private readonly AppSettingsConfig _settingsConfig = settingsConfig.Value;
        public bool TryGet<T>(object key, out T? value)
        {
            return cache.TryGetValue(key, out value);
        }

        public void Set<T>(object key, T value)
        {
            var options = GetMemoryCacheOptions();
            cache.Set(key, value, options);
        }

        private MemoryCacheEntryOptions GetMemoryCacheOptions()
        {
            var expirationTime = TimeSpan.FromSeconds(_settingsConfig.CacheSettings!.AbsoluteExpirationFromSeconds);
            return new MemoryCacheEntryOptions().SetAbsoluteExpiration(expirationTime);
        }
    }
}
