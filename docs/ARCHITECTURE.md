# Nexora Architecture

Nexora is structured as a full-stack web application optimized for maintainability, security, and enterprise SaaS scalability.

## Tech Stack

### Frontend
- **Framework**: Angular (Standalone Components)
- **State Management**: Signals, RxJS, and NgRx SignalStore
- **UI & Styling**: PrimeNG (Component foundation) + Tailwind CSS (Layout/Utilities)
- **Architecture**: Domain/feature-sliced folders with lazy-loaded routing.

### Backend
- **Framework**: ASP.NET Core Web API
- **Architecture**: Event-Driven Modular Monolith following Clean Architecture principles.
- **Project Structure**: Vertical Slicing (Single-Project Modules). Each module is a single `.csproj` containing `API`, `Application`, `Domain`, and `Persistence` folders. Modules communicate only via Domain Events via a `Shared` kernel.
- **Database**: PostgreSQL (via Entity Framework Core)
- **Primary Keys**: `Guid.CreateVersion7()` (UUIDv7) is strictly used for all entity primary keys to guarantee sequential, time-sortable inserts that prevent B-Tree index fragmentation and database page splits.
- **Patterns**: CQRS using MediatR, FluentValidation, Outbox Pattern (for reliable message delivery), and Inbox Pattern (for idempotent message processing).
- **Startup Strategy**: EF Core migrations and module-specific data seeding (`IDataSeeder`) are executed automatically and idempotently during application startup before HTTP requests are accepted.
- **API Documentation**: Swagger/OpenAPI via Swashbuckle (configured with Keycloak JWT Bearer authentication).
- **Real-time**: SignalR for collaborative updates.
- **Background Jobs**: Hangfire.
- **File Storage**: MinIO (S3-compatible).
- **Observability**: 
  - Centralized structured logging with Correlation IDs injected via middleware for end-to-end distributed request tracing.
  - Native Health Checks (`/api/health`) configured using Minimal APIs to concurrently probe PostgreSQL, Redis, MinIO, Keycloak, and Mailpit availability. This enforces a `503 Fail-Fast` pattern if infrastructure is degraded and correctly maps Microsoft's `HealthReport` to a safe JSON representation.

### Infrastructure
- Entirely Dockerized for development consistency (`docker-compose`).
- Containers for PostgreSQL, Redis, Mailpit (email testing), MinIO, Keycloak, and pgAdmin.
- Configuration for all services is managed securely via a local `.env` file injected into the containers.

## Core Rules
- **API Contracts**: Consistent camelCase JSON contracts across all endpoints.
- **Security First**: Continuous OWASP ASVS baseline checking, secure sessions, Keycloak IdP integration, and immutable audit trails.

