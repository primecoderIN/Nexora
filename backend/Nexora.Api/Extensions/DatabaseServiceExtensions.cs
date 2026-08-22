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
}
