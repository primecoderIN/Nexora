using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nexora.Modules.Identity.Domain.Entities;
using Nexora.Shared.Interfaces;

namespace Nexora.Modules.Identity.Persistence.Seeders;

/// <summary>
/// Responsible for seeding the initial Tenant and Admin User required 
/// for the application to function. 
/// </summary>
public class IdentityDataSeeder(IdentityDbContext dbContext, ILogger<IdentityDataSeeder> logger) : IDataSeeder
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Beginning Identity module data seeding...");

        // ------------------------------------------------------------------
        // 1. Seed Default Tenant
        // ------------------------------------------------------------------
        // We define a hardcoded GUID so that it is always the same across all developer machines.
        var defaultTenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        
        // IDEMPOTENCY CHECK: Does a Tenant with this ID already exist in PostgreSQL?
        var tenantExists = await dbContext.Tenants.AnyAsync(t => t.Id == defaultTenantId, cancellationToken);

        if (!tenantExists)
        {
            // If it does NOT exist, create exactly ONE Tenant.
            var defaultTenant = new Tenant(defaultTenantId, "Nexora Default Organization");
            
            dbContext.Tenants.Add(defaultTenant);
            logger.LogInformation("Seeded Default Tenant: {TenantName}", defaultTenant.Name);
        }

        // ------------------------------------------------------------------
        // 2. Seed Default Admin User
        // ------------------------------------------------------------------
        var adminEmail = "admin@nexora.com";
        
        // IDEMPOTENCY CHECK: Does a User with this email already exist?
        var userExists = await dbContext.Users.AnyAsync(u => u.Email == adminEmail, cancellationToken);

        if (!userExists)
        {
            // If it does NOT exist, create exactly ONE User.
            var adminUser = new User(
                id: Guid.Parse("00000000-0000-0000-0000-000000000002"), // This is the USER'S unique ID (ends in 2)
                identityId: "local-admin-sub", // In reality, this would match Keycloak's Subject ID
                email: adminEmail,
                firstName: "System",
                lastName: "Admin",
                tenantId: defaultTenantId // We assign this user to the Tenant we created above (ends in 1)
            );

            dbContext.Users.Add(adminUser);
            logger.LogInformation("Seeded Default Admin User: {AdminEmail}", adminUser.Email);
        }

        // Save all changes idempotently
        if (dbContext.ChangeTracker.HasChanges())
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Identity module data seeding completed and saved.");
        }
        else
        {
            logger.LogInformation("Identity module data seeding skipped (already up to date).");
        }
    }
}
