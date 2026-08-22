namespace Nexora.Shared.Interfaces;

/// <summary>
/// A generic interface for populating the database with required initial data.
/// Each module (e.g., Identity, Projects) should implement this interface 
/// to seed its own specific entities.
/// </summary>
public interface IDataSeeder
{
    /// <summary>
    /// Executes the seeding logic. Implementations MUST be idempotent.
    /// </summary>
    Task SeedAsync(CancellationToken cancellationToken = default);
}
