
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using NetDapperWebApi_local.Common.Interfaces;

namespace NetDapperWebApi_local.Services
{
    public class RedisCacheService(IDistributedCache cache) : IRedisCacheService
    {
        private readonly IDistributedCache _cache = cache;

        public T GetData<T>(string key)
        {
            var data =  _cache.GetString(key);

            if (string.IsNullOrEmpty(data))
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(data); // Sửa lỗi
        }

        public void SetData<T>(string key, T data)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5)
            };
            _cache?.SetString(key, JsonSerializer.Serialize(data), options);
        }
    }
}