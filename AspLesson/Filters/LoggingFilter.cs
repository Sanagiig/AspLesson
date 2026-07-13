using System.Diagnostics;

namespace AspLesson.Filters;

public class LoggingFilter(ILogger<LoggingFilter> logger) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var endpoint = context.HttpContext.GetEndpoint()?.DisplayName;
        var p = context.GetArgument<string>(0);
        logger.LogInformation("Executing: {Endpoint} with argument: {Argument}", endpoint, p);
        
        var stopwatch = Stopwatch.StartNew();
        var result = await next(context);
        stopwatch.Stop();

        logger.LogInformation("Completed: {Endpoint} in {Duration}ms",
            endpoint, stopwatch.ElapsedMilliseconds);

        return result;
    }
}