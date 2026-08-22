using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Nexora.Api.Middleware;

/// <summary>
/// Intercepts incoming HTTP requests to extract or generate a Correlation ID.
/// This ID is added to the logging scope so all subsequent logs for this request share the same ID.
/// </summary>
public class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
{
    private const string CorrelationIdHeader = "X-Correlation-ID";

    public async Task InvokeAsync(HttpContext context)
    {
        // 1. Try to get the Correlation ID from the incoming request header (e.g., set by Angular)
        if (!context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId))
        {
            // 2. If it doesn't exist, generate a new one
            correlationId = Guid.NewGuid().ToString();
        }

        // 3. Add it to the response headers so the client can track it as well
        context.Response.OnStarting(() =>
        {
            // Only add if it hasn't been added already by something else
            if (!context.Response.Headers.ContainsKey(CorrelationIdHeader))
            {
                context.Response.Headers.Append(CorrelationIdHeader, correlationId);
            }
            return Task.CompletedTask;
        });

        // 4. Push the Correlation ID into the logging context.
        // Any log statements written within this 'using' block will have the CorrelationId property attached.
        using (logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = correlationId.ToString()
        }))
        {
            await next(context);
        }
    }
}

public static class CorrelationIdMiddlewareExtensions
{
    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CorrelationIdMiddleware>();
    }
}
