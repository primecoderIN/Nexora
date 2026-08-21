namespace Nexora.Shared.Exceptions;

/// <summary>
/// Thrown when an authenticated user does not have permission to access a resource.
/// Maps to 403 Forbidden.
/// </summary>
public class ForbiddenAccessException : DomainException
{
    public ForbiddenAccessException(string message) : base(message)
    {
    }
}
