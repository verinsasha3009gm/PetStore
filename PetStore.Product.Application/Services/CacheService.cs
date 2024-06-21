using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using PetStore.Products.Domain.Interfaces.Services;
using System.Text;
using System.Text.Json;

namespace PetStore.Products.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }
        public T? Get<T>(string key)
        {
            var value = _cache.GetString(key);
            if (value != null)
            {
                return value.Length > 0 ? JsonConvert.DeserializeObject<T>(value) : default(T);
            }
            return default(T);
        }
        public T Set<T>(string key, T value)
        {
            var timeOut = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
                SlidingExpiration = TimeSpan.FromMinutes(60)
            };
            _cache.Set(key, System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(value), timeOut);
            return value;
        }
        public T? Delete<T>(string key)
        {
            var value = Get<T>(key);
            if (value == null)
            {
                return default;
            }
            _cache.Remove(key);
            return value;
        }
        public T? Refrech<T>(string key)
        {
            var value = _cache.GetString(key);
            if (value == null || value.Length < 1)
            {
                return default;
            }
            var Data = JsonConvert.DeserializeObject<T>(value);
            if (Data == null)
            {
                return default;
            }
            var timeOut = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
                SlidingExpiration = TimeSpan.FromMinutes(60)
            };
            _cache.Set(key, System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(value), timeOut);

            return Data;
        }
    }
}
