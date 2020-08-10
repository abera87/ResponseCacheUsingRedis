using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BackOffice.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int timetoLiveSeconds;

        public CacheAttribute(int timetoLiveSeconds)
        {
            this.timetoLiveSeconds = timetoLiveSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var configurationSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var redisEnable = Convert.ToBoolean(configurationSettings.GetSection("RedisConfig").GetSection("Enable").Value);
            
            if (!redisEnable)
            {
                await next();
                return;
            }

            // check if data available in cache and return from cache
            var cachedResponse = await cacheService.GetCacheResponseAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = (int)HttpStatusCode.OK
                };
                context.Result = contentResult;
                return;
            }
            var executedContext = await next();
            if (executedContext.Result is OkObjectResult okObjectResult)
            {
                await cacheService.SetCacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(timetoLiveSeconds));
            }

            // get actual value and cache it for next retrival
        }

        private static string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();

            keyBuilder.Append($"{request.Path}");

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}