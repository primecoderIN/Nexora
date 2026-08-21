# Authorization & Security Guide

> **Note**: This document is actively being fleshed out as we implement our Identity module.

## Identity Provider (Authentication)
We use **Keycloak** (OIDC/OAuth 2.0) as our external Identity Provider. It handles passwords, MFA, and SSO integrations. It issues a JWT which our ASP.NET Core API validates.

## Tenant Isolation & Onboarding
We use **Local Tenant Mapping** enforced via explicit onboarding:
1. Keycloak handles authentication and provides a unique `sub` identifier.
2. A user **must** either create a new Tenant (`POST /api/tenants`) or join an existing one (`POST /api/users/sync`) before they can use the application. **We do not implicitly create Personal Tenants.**
3. Once synced, the `Nexora.Modules.Identity` maps the Keycloak `sub` to a local `UserId` and `TenantId`.
4. The `TenantId` is stored in a scoped `ICurrentUserContext` (in `Nexora.Shared`).
5. **Entity Framework Core Global Query Filters** automatically append `WHERE TenantId = @id` to all database queries, making cross-tenant data leakage impossible at the database level.
   
   This is implemented directly in the DbContext:
   ```csharp
   // MULTI-TENANCY: Global Query Filter
   builder.HasQueryFilter(u => u.TenantId == currentUserContext.TenantId);
   ```

## RBAC Model
*(Coming soon)*

## BOLA/IDOR Prevention
*(Coming soon)*
