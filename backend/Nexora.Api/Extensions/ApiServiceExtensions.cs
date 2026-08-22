using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Nexora.Api.Extensions;

/// <summary>
/// Responsibility: Configures services related to the API presentation layer.
/// This includes API endpoints exploration, routing rules, controller JSON serialization settings,
/// CORS policies, and global exception handling.
/// </summary>
public static class ApiServiceExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();

        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
        });

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase; //Use camel case for JSON property names in response
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true; //Accept any case of property names in request body
            });

        // CORS origins are read from configuration so the same binary can serve
        // different environments (dev / staging / prod) without recompilation.
        var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [];

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithOrigins(allowedOrigins);
            });
        });

        // Register Global Exception Handler (.NET 8+ standard)
        services.AddExceptionHandler<Nexora.Api.Middleware.GlobalExceptionHandler>();
        services.AddProblemDetails();
        
        return services;
    }
}
