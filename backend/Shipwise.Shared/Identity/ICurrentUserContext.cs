namespace Shipwise.Shared.Identity;

public interface ICurrentUserContext
{
    /// <summary>
    /// Gets the unique identifier of the currently authenticated user.
    /// </summary>
    Guid UserId { get; }

    /// <summary>
    /// Gets the unique identifier of the tenant the user is currently operating in.
    /// Used for strict data isolation in global query filters.
    /// </summary>
    Guid TenantId { get; }

    /// <summary>
    /// Indicates whether there is an authenticated user for the current request.
    /// </summary>
    bool IsAuthenticated { get; }
}
