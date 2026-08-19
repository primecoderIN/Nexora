# Authorization & Security Guide

> **Note**: This document is actively being fleshed out as we implement our Identity module.

## Identity Provider (Authentication)
We use **Keycloak** (OIDC/OAuth 2.0) as our external Identity Provider. It handles passwords, MFA, and SSO integrations. It issues a JWT which our ASP.NET Core API validates.

## Tenant Isolation
We use **Local Tenant Mapping**:
1. Keycloak provides a unique `sub` identifier.
2. The `Shipwise.Modules.Identity` maps that `sub` to a local `TenantId`.
3. The `TenantId` is stored in a scoped `ICurrentUserContext` (in `Shipwise.Shared`).
4. **Entity Framework Core Global Query Filters** automatically append `WHERE TenantId = @id` to all database queries, making cross-tenant data leakage impossible at the database level.

## RBAC Model
*(Coming soon)*

## BOLA/IDOR Prevention
*(Coming soon)*
