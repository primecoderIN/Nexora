namespace Nexora.Shared.Exceptions;

/// <summary>
/// Exception thrown when a requested resource is not found.
/// </summary>
public class NotFoundException : DomainException
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string resourceName, object key) 
        : base($"Entity \"{resourceName}\" ({key}) was not found.")
    {
    }
}
