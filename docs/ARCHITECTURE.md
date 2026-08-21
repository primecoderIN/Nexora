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
- **Patterns**: CQRS using MediatR, FluentValidation.
- **Real-time**: SignalR for collaborative updates.
- **Background Jobs**: Hangfire.
- **File Storage**: MinIO (S3-compatible).

### Infrastructure
- Entirely Dockerized for development consistency (`docker-compose`).
- Containers for PostgreSQL, Redis, Mailpit (email testing), and MinIO.

## Core Rules
- **API Contracts**: Consistent camelCase JSON contracts across all endpoints.
- **Security First**: Continuous OWASP ASVS baseline checking, secure sessions, and immutable audit trails.
