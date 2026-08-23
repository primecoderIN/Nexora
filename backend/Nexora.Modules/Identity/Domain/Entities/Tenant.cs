namespace Nexora.Modules.Identity.Domain.Entities;

/// <summary>
/// Represents a tenant (organization or workspace) within the system.
/// All multi-tenant data is isolated by the TenantId.
/// </summary>
/// <param name="id">The unique identifier of the tenant.</param>
/// <param name="name">The display name of the tenant.</param>
public class Tenant(Guid id, string name)
{
    public Guid Id { get; private set; } = id; // This will act as our TenantId
    public string Name { get; private set; } = name;
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

    // Navigation property
    public ICollection<User> Users { get; private set; } = new List<User>();

    private Tenant() : this(Guid.Empty, string.Empty) { }
}
