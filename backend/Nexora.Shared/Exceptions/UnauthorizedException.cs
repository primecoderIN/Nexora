namespace Nexora.Shared.Exceptions;

/// <summary>
/// Thrown when a user is not authenticated.
/// Maps to 401 Unauthorized.
/// </summary>
public class UnauthorizedException : DomainException
{
    public UnauthorizedException(string message) : base(message)
    {
    }
}
