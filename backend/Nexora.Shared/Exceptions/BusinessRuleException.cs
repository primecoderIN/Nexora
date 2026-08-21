namespace Nexora.Shared.Exceptions;

/// <summary>
/// Thrown when a core domain business rule is violated.
/// Maps to 409 Conflict.
/// </summary>
public class BusinessRuleException : DomainException
{
    public BusinessRuleException(string message) : base(message)
    {
    }
}
