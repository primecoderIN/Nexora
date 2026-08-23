namespace Nexora.Shared.Exceptions;

/// <summary>
/// Exception thrown when a requested resource is not found.
/// </summary>
public class NotFoundException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public NotFoundException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class for a specific resource and key.
    /// </summary>
    /// <param name="resourceName">The name of the resource that was not found.</param>
    /// <param name="key">The identifier or key of the resource that was not found.</param>
    public NotFoundException(string resourceName, object key) 
        : base($"Entity \"{resourceName}\" ({key}) was not found.")
    {
    }
}
