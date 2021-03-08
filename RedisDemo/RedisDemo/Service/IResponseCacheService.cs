using System;
using System.Threading.Tasks;

namespace RedisDemo.Service
{
    public interface IResponseCacheService
    {
        Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeOut);
        Task<string> GetCacheResponseAsync(string cacheKey);
        Task RemoveCachResponseAsync(string partern);

    }
}
