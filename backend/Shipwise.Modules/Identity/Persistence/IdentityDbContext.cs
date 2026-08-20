using Microsoft.EntityFrameworkCore;
using Shipwise.Modules.Identity.Domain.Entities;

namespace Shipwise.Modules.Identity.Persistence;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : DbContext(options)
{
    public DbSet<Tenant> Tenants { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Best Practice: Modular Monolith Schema Isolation
        modelBuilder.HasDefaultSchema("identity");

        modelBuilder.Entity<Tenant>(builder =>
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(256);
            
            // 1-to-many relationship
            builder.HasMany(t => t.Users)
                   .WithOne(u => u.Tenant)
                   .HasForeignKey(u => u.TenantId)
                   .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.IdentityId).IsRequired().HasMaxLength(256);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
            builder.Property(u => u.FirstName).IsRequired().HasMaxLength(128);
            builder.Property(u => u.LastName).IsRequired().HasMaxLength(128);

            builder.HasIndex(u => u.IdentityId).IsUnique(); // Keycloak 'sub' must be unique
            builder.HasIndex(u => u.Email).IsUnique();
        });
    }
}
