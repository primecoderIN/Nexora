# Authorization & Security Guide

## Identity Provider (Authentication)
We use **Keycloak** (OIDC/OAuth 2.0) as our external Identity Provider. It handles passwords, MFA, and SSO integrations. It issues a JWT which our ASP.NET Core API validates.

## Tenant Isolation & Onboarding (Multi-Tenancy)
We use **Local Tenant Mapping** enforced via explicit onboarding:
1. Keycloak handles authentication and provides a unique `sub` identifier in the JWT.
2. A user **must** either create a new Tenant (`POST /api/tenants`) or join an existing one before they can use the application. **We do not implicitly create Personal Tenants.**
3. Our custom ASP.NET Core Middleware extracts the `sub` claim from the JWT Bearer token on every request.
4. The `Nexora.Modules.Identity` maps the Keycloak `sub` to a local `UserId` and `TenantId`.
5. The `TenantId` is injected into a scoped `ICurrentUserContext` (located in `Nexora.Shared`).

## BOLA/IDOR Prevention (Global Query Filters)
Broken Object Level Authorization (BOLA/IDOR) is the most critical API vulnerability. Instead of relying on developers to remember to append `WHERE TenantId = @id` to every single query, we enforce it architecturally at the lowest level.

**Entity Framework Core Global Query Filters** automatically append the TenantId check to all database queries, making cross-tenant data leakage impossible.

This is implemented directly in the `IdentityDbContext` (and all future module contexts):
```csharp
// MULTI-TENANCY: Global Query Filter
// We inject the scoped ICurrentUserContext into the DbContext constructor.
builder.HasQueryFilter(u => u.TenantId == currentUserContext.TenantId);
```
*Note: If you are building a background worker or startup seeder that needs to query across tenants, you must explicitly append `.IgnoreQueryFilters()` to your LINQ query.*

## RBAC Model
*(Coming soon - to be implemented using Keycloak Roles mapped to MediatR Authorization Behaviors)*
