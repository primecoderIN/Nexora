# Project Folder Structure

Nexora is built using an **Event-Driven Modular Monolith** approach. We utilize **Single-Project Modules** (often called Vertical Slicing) to enforce Clean Architecture boundaries while avoiding the "project explosion" commonly seen in massive enterprise solutions.

Below is the complete annotated directory tree.

```text
Nexora/
├── backend/
│   ├── Nexora.slnx                               ← Master solution file
│   │
│   ├── Nexora.Api/                               ← 1. Presentation Layer (Host)
│   │   ├── Extensions/                             ← Service registrations (Swagger, EF Core, etc.)
│   │   ├── Middleware/                             ← Global exception handling
│   │   ├── Program.cs                              ← Composition Root & entry point
│   │   └── Nexora.Api.csproj
│   │
│   ├── Nexora.Shared/                            ← 2. Shared Kernel
│   │   ├── Exceptions/                             ← Domain exceptions (NotFound, BusinessRule, Unauthorized, etc.)
│   │   ├── Interfaces/                             ← ICurrentUserContext, shared contracts
│   │   ├── Validation/                             ← MediatR ValidationBehavior pipeline
│   │   └── Nexora.Shared.csproj                  (References nothing)
│   │
│   └── Nexora.Modules/                           ← 3. Isolated Business Modules
│       │
│       ├── Identity/
│       │   └── Nexora.Modules.Identity/          ← Single C# Project per Module
│       │       ├── API/                            ← Controllers and endpoints
│       │       ├── Application/                    ← MediatR commands, queries, and DTOs
│       │       ├── Domain/                         ← Core Entities, Enums, and Domain Events
│       │       ├── Persistence/                    ← Module-specific DbContext and Migrations
│       │       └── Nexora.Modules.Identity.csproj (References Nexora.Shared)
│       │
│       ├── Releases/
│       │   └── Nexora.Modules.Releases/
│       │       ├── API/
│       │       ├── Application/
│       │       ├── Domain/
│       │       ├── Persistence/
│       │       └── Nexora.Modules.Releases.csproj (References Nexora.Shared)
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
1. **The Shared Kernel**: `Nexora.Shared` contains base classes and events. It cannot reference any other project in the solution.
2. **Module Isolation**: A module (e.g., `Nexora.Modules.Identity`) can ONLY reference `Nexora.Shared`. It cannot directly reference `Nexora.Modules.Releases`.
3. **Cross-Module Communication**: If `Releases` needs to know when a user signs up, it must listen for a `UserRegisteredEvent` published by `Identity` through the `Shared` kernel's event bus.
4. **The Composition Root**: `Nexora.Api` is the only project allowed to reference everything. It wires up the database contexts, dependency injection, and middleware.
