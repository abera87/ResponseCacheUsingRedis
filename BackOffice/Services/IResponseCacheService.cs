using System;
using System.Threading.Tasks;

namespace BackOffice.Services
{
    public interface IResponseCacheService
    {
        Task SetCacheResponseAsync(string cacheKey,object response,TimeSpan liveTime);

        Task<string> GetCacheResponseAsync(string cacheKey);
    }
}