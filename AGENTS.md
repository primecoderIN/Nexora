# Nexora Global Agent Rules

## C# Coding Standards & Best Practices

1. **C# 12 Primary Constructors**
   - **Rule**: ALWAYS use C# 12 Primary Constructors for dependency injection and entity definition.
   - **Avoid**: Do not declare private `readonly` fields and assign them in an explicit constructor unless you need complex validation logic inside the constructor body that cannot be handled by a primary constructor.
   - **Example**:
     ```csharp
     // Good
     public class MyService(IDbContext dbContext, ILogger<MyService> logger) { }
     
     // Bad
     public class MyService 
     { 
         private readonly IDbContext _dbContext;
         public MyService(IDbContext dbContext) { _dbContext = dbContext; }
     }
     ```
   - **EF Core Note**: If Entity Framework Core requires a parameterless constructor, define it as `private MyClass() : this(...) { }` alongside the primary constructor.

2. **Clean Program.cs (Service Extensions)**
   - **Rule**: Keep `Program.cs` as absolutely thin as possible. Do not register services directly in `Program.cs`.
   - **Implementation**: Group related services and register them via extension methods on `IServiceCollection` inside an `Extensions/` folder (e.g., `AddApiServices()`, `AddIdentityServices()`, `AddDatabaseServices()`).
   - **Why**: It improves readability, makes testing easier, and organizes dependencies by module/domain.

3. **Ubiquitous Language (Naming Consistency)**
   - **Rule**: Always use the exact domain entity name for properties and foreign keys across all layers of the application. Do NOT mix generic architectural terms with domain terms.
   - **Example**: If the domain entity is `Tenant`, the property must be `TenantId` everywhere (in Contexts, Interfaces, DTOs, and EF Core Configurations). Do not use `OrganizationId` in interfaces and `TenantId` in domain models.
