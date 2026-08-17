# Shipwise Architecture

Shipwise is structured as a full-stack web application optimized for maintainability, security, and enterprise SaaS scalability.

## Tech Stack

### Frontend
- **Framework**: Angular (Standalone Components)
- **State Management**: Signals, RxJS, and NgRx SignalStore
- **UI & Styling**: PrimeNG (Component foundation) + Tailwind CSS (Layout/Utilities)
- **Architecture**: Domain/feature-sliced folders with lazy-loaded routing.

### Backend
- **Framework**: ASP.NET Core Web API
- **Architecture**: Modular Monolith following Clean Architecture principles.
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
