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
        
        // Register Health Checks (Liveness/Readiness probes)
        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("DefaultConnection not found"), name: "PostgreSQL")
            .AddRedis(configuration.GetConnectionString("Redis") ?? throw new InvalidOperationException("Redis not found"), name: "Redis")
            .AddUrlGroup(new Uri(configuration.GetConnectionString("MinIO") ?? throw new InvalidOperationException("MinIO not found")), name: "MinIO")
            .AddUrlGroup(new Uri(configuration["HealthChecks:Keycloak"] ?? throw new InvalidOperationException("Keycloak Health URL not found")), name: "Keycloak")
            .AddUrlGroup(new Uri(configuration["HealthChecks:Mailpit"] ?? throw new InvalidOperationException("Mailpit Health URL not found")), name: "Mailpit");

        return services;
    }

    public static void MapCustomHealthChecks(this WebApplication app)
    {
        app.MapGet("/api/health", async (Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckService healthCheckService) =>
        {
            var report = await healthCheckService.CheckHealthAsync();
            var response = new
            {
                status = report.Status.ToString(),
                totalDuration = report.TotalDuration,
                entries = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    error = e.Value.Exception?.Message
                })
            };

            return report.Status == Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy 
                ? Microsoft.AspNetCore.Http.Results.Ok(response) 
                : Microsoft.AspNetCore.Http.Results.Json(response, statusCode: 503);
        })
        .WithTags("System Health")
        .WithSummary("API Health Check")
        .WithDescription("Checks the health of the Nexora API and its dependencies.");
    }
}
