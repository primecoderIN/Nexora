namespace Nexora.Modules.Identity.Domain.Entities;

public class User(Guid id, string identityId, string email, string firstName, string lastName, Guid tenantId)
{
    public Guid Id { get; private set; } = id;
    public string IdentityId { get; private set; } = identityId; // The Keycloak 'sub' claim
    public string Email { get; private set; } = email;
    public string FirstName { get; private set; } = firstName;
    public string LastName { get; private set; } = lastName;

    // Each user belongs to exactly one Tenant
    public Guid TenantId { get; private set; } = tenantId;
    public Tenant Tenant { get; private set; } = null!;

    private User() : this(Guid.Empty, string.Empty, string.Empty, string.Empty, string.Empty, Guid.Empty) { } // EF Core requires a parameterless constructor
}
