using Microsoft.EntityFrameworkCore;
using Shipwise.Modules.Identity.Persistence;

namespace Shipwise.Api.Extensions;

public static class DatabaseServiceExtensions
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<IdentityDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "identity");
            });
        });

        return services;
    }
}
