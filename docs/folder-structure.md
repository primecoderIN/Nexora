# Project Folder Structure

Shipwise is built using an **Event-Driven Modular Monolith** approach. We utilize **Single-Project Modules** (often called Vertical Slicing) to enforce Clean Architecture boundaries while avoiding the "project explosion" commonly seen in massive enterprise solutions.

Below is the complete annotated directory tree.

```text
Shipwise/
├── backend/
│   ├── Shipwise.slnx                               ← Master solution file
│   │
│   ├── Shipwise.Api/                               ← 1. Presentation Layer (Host)
│   │   ├── Extensions/                             ← Service registrations (Swagger, EF Core, etc.)
│   │   ├── Middleware/                             ← Global exception handling
│   │   ├── Program.cs                              ← Composition Root & entry point
│   │   └── Shipwise.Api.csproj
│   │
│   ├── Shipwise.Shared/                            ← 2. Shared Kernel
│   │   ├── Behaviors/                              ← MediatR validation pipelines
│   │   ├── Common/                                 ← ApiResponse envelope, Result<T> pattern
│   │   ├── Exceptions/                             ← Standard domain exceptions (NotFound, Conflict)
│   │   ├── Interfaces/                             ← IDomainEvent, IRepository
│   │   └── Shipwise.Shared.csproj                  (References nothing)
│   │
│   └── Shipwise.Modules/                           ← 3. Isolated Business Modules
│       │
│       ├── Identity/
│       │   └── Shipwise.Modules.Identity/          ← Single C# Project per Module
│       │       ├── API/                            ← Controllers and endpoints
│       │       ├── Application/                    ← MediatR commands, queries, and DTOs
│       │       ├── Domain/                         ← Core Entities, Enums, and Domain Events
│       │       ├── Persistence/                    ← Module-specific DbContext and Migrations
│       │       └── Shipwise.Modules.Identity.csproj (References Shipwise.Shared)
│       │
│       ├── Releases/
│       │   └── Shipwise.Modules.Releases/
│       │       ├── API/
│       │       ├── Application/
│       │       ├── Domain/
│       │       ├── Persistence/
│       │       └── Shipwise.Modules.Releases.csproj (References Shipwise.Shared)
│       │
│       └── [Future modules...]
│
├── frontend/                                       ← Angular Standalone Application
│   └── src/
│       ├── app/
│       │   ├── core/                               ← Auth, Interceptors, generic services
│       │   ├── features/                           ← Domain-sliced components (matching backend modules)
│       │   ├── shared/                             ← UI primitives, Design System components
│       │   └── app.routes.ts                       ← Lazy-loaded routing
│       └── styles/                                 ← Tailwind configuration and PrimeNG overrides
│
├── knowledge-base/                                 ← Development history and phase tracking
└── docs/                                           ← Permanent architectural documentation
```

## Architectural Rules
1. **The Shared Kernel**: `Shipwise.Shared` contains base classes and events. It cannot reference any other project in the solution.
2. **Module Isolation**: A module (e.g., `Shipwise.Modules.Identity`) can ONLY reference `Shipwise.Shared`. It cannot directly reference `Shipwise.Modules.Releases`.
3. **Cross-Module Communication**: If `Releases` needs to know when a user signs up, it must listen for a `UserRegisteredEvent` published by `Identity` through the `Shared` kernel's event bus.
4. **The Composition Root**: `Shipwise.Api` is the only project allowed to reference everything. It wires up the database contexts, dependency injection, and middleware.
