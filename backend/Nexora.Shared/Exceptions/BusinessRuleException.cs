namespace Nexora.Shared.Exceptions;

/// <summary>
/// Thrown when a core domain business rule is violated.
/// Maps to 409 Conflict.
/// </summary>
public class BusinessRuleException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessRuleException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public BusinessRuleException(string message) : base(message)
    {
    }
}
