using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nexora.Modules.Identity.Persistence;

namespace Nexora.Modules.Identity.API.Middleware;

/// <summary>
/// Middleware responsible for extracting the Keycloak 'sub' claim from the JWT,
/// looking up the user in the database, and caching their internal UserId and TenantId
/// for the duration of the HTTP request.
/// </summary>
public class TenantResolutionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // STEP 1: Check if the request has a valid, authenticated JWT token.
        // If the user isn't logged in, we skip the database lookup completely to save performance.
        if (context.User.Identity?.IsAuthenticated == true)
        {
            // STEP 2: Extract the unique Keycloak Identity ID (the 'sub' claim) from the token.
            // This is the bridge between our authentication server (Keycloak) and our physical database.
            var subClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (!string.IsNullOrEmpty(subClaim))
            {
                // STEP 3: Resolve the database context for this specific HTTP request.
                // We use RequestServices because this middleware acts as a singleton-like pipeline,
                // but the DbContext must be scoped per request.
                var dbContext = context.RequestServices.GetRequiredService<IdentityDbContext>();

                // STEP 4: Query the database to find the user's Nexora account and TenantId.
                // CRITICAL ARCHITECTURE DECISION: We MUST use IgnoreQueryFilters() here.
                // If we don't, Entity Framework will try to apply the global TenantId filter,
                // which will call CurrentUserContext, which will try to read the TenantId we haven't set yet,
                // resulting in an infinite circular dependency loop.
                var user = await dbContext.Users
                    .IgnoreQueryFilters()
                    .AsNoTracking() // We only need to read the ID, we aren't updating the user here.
                    .FirstOrDefaultAsync(u => u.IdentityId == subClaim);

                // STEP 5: If the user exists in our database, cache their IDs.
                if (user != null)
                {
                    // By placing these in HttpContext.Items, we cache them for the exact lifespan of this HTTP request.
                    // This means CurrentUserContext can read them instantly without ever hitting the database again.
                    context.Items["UserId"] = user.Id;
                    context.Items["TenantId"] = user.TenantId;
                }
            }
        }

        // STEP 6: Pass the context down to the next middleware (e.g., Controllers)
        await next(context);
    }
}

public static class TenantResolutionMiddlewareExtensions
{
    public static IApplicationBuilder UseTenantResolutionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TenantResolutionMiddleware>();
    }
}
