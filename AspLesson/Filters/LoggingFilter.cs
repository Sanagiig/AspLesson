using System.Diagnostics;

namespace AspLesson.Filters;

/// <summary>
/// Minimal API 端点日志过滤器（仅适用于 MapGet/MapPost 等端点）。
/// MVC Controller 请使用 ValidationActionFilter。
/// </summary>
public class LoggingFilter(ILogger<LoggingFilter> logger) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var endpoint = context.HttpContext.GetEndpoint()?.DisplayName;

        logger.LogInformation("Executing: {Endpoint}", endpoint);

        var stopwatch = Stopwatch.StartNew();
        var result = await next(context);
        stopwatch.Stop();

        logger.LogInformation("Completed: {Endpoint} in {Duration}ms",
            endpoint, stopwatch.ElapsedMilliseconds);

        return result;
    }
}
