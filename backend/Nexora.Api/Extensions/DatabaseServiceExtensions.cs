using Microsoft.EntityFrameworkCore;
using Nexora.Modules.Identity.Persistence;

namespace Nexora.Api.Extensions;

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
