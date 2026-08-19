namespace Shipwise.Modules.Identity.Domain.Entities;

public class Tenant(Guid id, string name)
{
    public Guid Id { get; private set; } = id; // This will act as our TenantId
    public string Name { get; private set; } = name;

    // Navigation property
    public ICollection<User> Users { get; private set; } = new List<User>();

    private Tenant() : this(Guid.Empty, string.Empty) { }
}
