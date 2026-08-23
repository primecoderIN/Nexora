namespace Nexora.Modules.Identity.Domain.Entities;

/// <summary>
/// Represents an authenticated user in the system.
/// Users are synchronized with the external Identity Provider (e.g., Keycloak).
/// </summary>
/// <param name="id">The unique internal identifier for the user.</param>
/// <param name="identityId">The unique subject (sub) claim from the Identity Provider.</param>
/// <param name="email">The user's email address.</param>
/// <param name="firstName">The user's first name.</param>
/// <param name="lastName">The user's last name.</param>
/// <param name="tenantId">The identifier of the tenant this user belongs to.</param>
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
