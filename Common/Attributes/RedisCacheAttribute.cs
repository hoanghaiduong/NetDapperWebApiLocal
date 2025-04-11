using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;

namespace NetDapperWebApi.Common.Attributes
{
    public class RedisCacheAttribute : Attribute
    {
        // private readonly int _durationInSeconds;

        // public RedisCacheAttribute(int durationInSeconds)
        // {
        //     _durationInSeconds = durationInSeconds;
        // }

        // public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        // {
        //     var cache = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();
        //     var cacheKey = $"NetDapperWebApi_{context.HttpContext.Request.Path}";

        //     Console.WriteLine($"🔍 Checking cache for key: {cacheKey}");

        //     var cachedData = await cache.GetStringAsync(cacheKey);
        //     if (!string.IsNullOrEmpty(cachedData))
        //     {
        //         try
        //         {
        //             var result = JsonSerializer.Deserialize<object>(cachedData);
        //             Console.WriteLine("✅ Cache hit!");
        //             context.Result = new JsonResult(result);
        //             return;
        //         }
        //         catch (JsonException ex)
        //         {
        //             Console.WriteLine($"❌ Error deserializing cache data: {ex.Message}");
        //         }
        //     }

        //     Console.WriteLine("⚠ Cache miss! Fetching from database...");
        //     var executedContext = await next();

        //     if (executedContext.Result is ObjectResult objectResult)
        //     {
        //         var responseData = JsonSerializer.Serialize(objectResult.Value);
        //         var options = new DistributedCacheEntryOptions
        //         {
        //             AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_durationInSeconds)
        //         };

        //         await cache.SetStringAsync(cacheKey, responseData, options);

        //         // Kiểm tra lại sau khi lưu vào cache
        //         var checkCache = await cache.GetStringAsync(cacheKey);
        //         Console.WriteLine($"🔄 Cache stored successfully: {checkCache?.Substring(0, Math.Min(100, checkCache.Length))}...");
        //     }
        // }
    }
}
