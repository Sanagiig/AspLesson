using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace AspLesson.Filters;

public class CacheResourceFilter(IMemoryCache cache, ILogger logger) : IAsyncResourceFilter
{
    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        var cacheKey = context.HttpContext.Request.Path.ToString();

        if (cache.TryGetValue(cacheKey, out var cachedResult))
        {
            context.Result = new OkObjectResult(cachedResult);
            logger.LogInformation("Cache hit for {CacheKey}", cacheKey);
            return; // Short-circuit - skip the entire pipeline
        }

        var executedContext = await next();

        if (executedContext.Result is ObjectResult { Value: not null } objectResult)
        {
            cache.Set(cacheKey, objectResult.Value, TimeSpan.FromMinutes(5));
        }
    }
}