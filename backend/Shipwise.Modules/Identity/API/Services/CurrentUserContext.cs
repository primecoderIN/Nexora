using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Shipwise.Shared.Identity;

namespace Shipwise.Modules.Identity.API.Services;

public class CurrentUserContext(IHttpContextAccessor httpContextAccessor) : ICurrentUserContext
{
    // PERFORMANCE OPTIMIZATION: Scoped Caching
    // This service is registered as AddScoped(), meaning it lives for exactly one HTTP request.
    // We cache the TenantId and UserId so we don't hit the database multiple times per request.
    // If 4 different components ask for TenantId during a single API call, the DB is only queried once.
    // When the HTTP request finishes, the Garbage Collector destroys this class, preventing data leakage.
    private Guid? _tenantId;
    private Guid? _userId;

    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public Guid UserId 
    {
        get 
        {
            if (_userId.HasValue)
                return _userId.Value;

            if (!IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated.");

            // In a real implementation, we would extract the "sub" claim,
            // query our local database to find the internal Shipwise Guid,
            // and cache it.
            var subClaim = httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            // For now, returning an empty Guid until EF Core is fully wired up.
            _userId = Guid.Empty;
            return _userId.Value;
        }
    }

    public Guid TenantId 
    {
        get 
        {
            if (_tenantId.HasValue)
                return _tenantId.Value;

            if (!IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated.");

            // In a real implementation, we would look up the user's current active 
            // Tenant in the database based on their UserId.
            // For now, returning an empty Guid until EF Core is fully wired up.
            _tenantId = Guid.Empty;
            return _tenantId.Value;
        }
    }
}
