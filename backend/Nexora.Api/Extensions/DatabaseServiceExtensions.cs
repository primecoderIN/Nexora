using Microsoft.EntityFrameworkCore;
using Nexora.Modules.Identity.Persistence;

namespace Nexora.Api.Extensions;

/// <summary>
/// Responsibility: Configures database connections and Entity Framework Core DbContexts.
/// This includes registering DbContexts with the dependency injection container,
/// configuring connection strings, and setting up migration history schemas.
/// </summary>
public static class DatabaseServiceExtensions
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Retrieve the default connection string from appsettings.json or Environment Variables.
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Register the DbContext for the Identity Module.
        services.AddDbContext<IdentityDbContext>(options =>
        {
            // Configure EF Core to use PostgreSQL.
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                // Isolate migrations for this specific module into its own database schema ("identity")
                // to enforce the Modular Monolith boundary at the database level.
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "identity");
            });
        });

        return services;
    }

    /// <summary>
    /// Applies any pending Entity Framework Core migrations on application startup.
    /// In a massive distributed production environment, this should ideally be moved 
    /// to a CI/CD pipeline step (or Init Container) to prevent migration race conditions.
    /// </summary>
    public static async Task ApplyDatabaseMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<WebApplication>>();

        try
        {
            var identityContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
            
            // Wait for DB to be ready, then apply migrations. 
            // In a real microservices scenario, we'd use a Polly retry policy here.
            logger.LogInformation("Applying Identity Module database migrations...");
            await identityContext.Database.MigrateAsync();
            logger.LogInformation("Identity Module database migrations applied successfully.");
            
            // Future DbContexts (like ApplicationDbContext) will be migrated here as well.
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while applying database migrations.");
            throw; // Fail fast if we can't migrate the database
        }
    }
}
