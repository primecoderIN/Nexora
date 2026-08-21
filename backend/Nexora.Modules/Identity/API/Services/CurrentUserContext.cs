using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Nexora.Shared.Interfaces;

namespace Nexora.Modules.Identity.API.Services;

public class CurrentUserContext(IHttpContextAccessor httpContextAccessor) : ICurrentUserContext
{
    // PERFORMANCE OPTIMIZATION: Scoped Caching
    // This service is registered as AddScoped(), meaning it lives for exactly one HTTP request.
    // We cache the TenantId and UserId so we don't hit the database multiple times per request.
    // If 4 different components ask for TenantId during a single API call, the DB is only queried once.
    // When the HTTP request finishes, the Garbage Collector destroys this class, preventing data leakage.
    // STEP 1: Determine if the HTTP Request has a valid authentication token.
    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    // STEP 2: Expose the user's Nexora account ID.
    public Guid UserId 
    {
        get 
        {
            if (!IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated.");

            // We check the HttpContext.Items dictionary. This dictionary only lives for the duration of a single HTTP request.
            // Our TenantResolutionMiddleware already populated this when it extracted the token and queried the database.
            // By reading it from here, we avoid hitting the database multiple times in a single request.
            if (httpContextAccessor.HttpContext?.Items["UserId"] is Guid userId)
                return userId;

            // If it's not in the context items, it means the middleware didn't find the user in our database.
            // This is perfectly normal! It means the user is logged into Keycloak, but they haven't synced their profile
            // into Nexora yet (e.g. they are in the 'Onboarding' phase).
            return Guid.Empty;
        }
    }

    // STEP 3: Expose the user's active Tenant ID for global query filters.
    public Guid TenantId 
    {
        get 
        {
            if (!IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated.");

            // Just like UserId, we read this from the fast, in-memory HttpContext dictionary that the middleware populated.
            // Entity Framework Core will automatically call this property on EVERY database query.
            if (httpContextAccessor.HttpContext?.Items["TenantId"] is Guid tenantId)
                return tenantId;

            // If this is empty, it means the user has not been assigned a Tenant yet.
            // Our global query filters will automatically evaluate to "WHERE TenantId = '00000000-0000-0000-0000-000000000000'".
            // This guarantees they will see exactly 0 records across the entire system until they join an organization,
            // preventing massive data breaches!
            return Guid.Empty;
        }
    }
}
