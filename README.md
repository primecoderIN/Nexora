<div align="center">
  <h1>Shipwise</h1>
  <p><strong>A Next-Generation, Release-Centric Engineering Management Platform</strong></p>

  <p>
    <img src="https://img.shields.io/badge/.NET_10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 10" />
    <img src="https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white" alt="Angular" />
    <img src="https://img.shields.io/badge/TypeScript-007ACC?style=for-the-badge&logo=typescript&logoColor=white" alt="TypeScript" />
    <img src="https://img.shields.io/badge/PostgreSQL-336791?style=for-the-badge&logo=postgresql&logoColor=white" alt="PostgreSQL" />
    <img src="https://img.shields.io/badge/Docker-2CA5E0?style=for-the-badge&logo=docker&logoColor=white" alt="Docker" />
  </p>
</div>

<br />

Shipwise is a full-stack, enterprise-grade engineering management platform built for modern SaaS. Unlike traditional issue trackers, Shipwise treats the **Release** as the primary workspace, connecting backlogs, code reviews, manual approvals, and deployments into a single unified timeline.

---

## ✨ Key Capabilities

Shipwise isn't just a basic CRUD app; it is engineered to solve complex organizational problems using modern architectural patterns:

- 🏗️ **Event-Driven Modular Monolith**: The backend is highly cohesive but loosely coupled. Business domains are partitioned into Single-Project Modules (Vertical Slicing) enforcing Clean Architecture, ensuring strict isolation without project bloat.
- 🔐 **Advanced Tenant & Authorization Security**: A custom RBAC engine and robust feature flag system (tenant/user/beta) strictly gates resources. Organizations are isolated to prevent cross-tenant data bleeding (BOLA/IDOR prevention).
- 🎨 **State-of-the-art White-Label UI**: A responsive, accessible component system built on PrimeNG and Tailwind CSS, featuring dynamic theming and white-labeling for enterprise clients.
- ⚡ **Real-Time Collaboration**: Powered by SignalR, ensuring that task updates, approvals, and release health metrics are synchronized across all active users instantly.
- 🚀 **Release-Centric Lifecycles**: Manage the entire lifecycle of software delivery—from backlog to post-go-live tracking—within independent, isolated release environments.

---

## 🛠️ Technology Stack

**Backend (ASP.NET Core)**
* **API Engine**: ASP.NET Core Web API
* **Architecture**: Clean Architecture & CQRS (Command Query Responsibility Segregation)
* **Mediation**: MediatR (with FluentValidation pipeline behaviors)
* **Data Access**: Entity Framework Core (EF Core)
* **Database**: PostgreSQL
* **Real-time & Jobs**: SignalR, Hangfire
* **Storage**: MinIO (S3-compatible)

**Frontend (Angular SPA)**
* **Core**: Angular (Standalone Components), TypeScript
* **State Management**: Signals, RxJS, NgRx SignalStore
* **Styling**: Tailwind CSS
* **UI Primitives**: PrimeNG, Angular CDK

---

## 🚀 Getting Started

The entire Shipwise platform is fully containerized for a friction-free developer experience.

### 1. Clone the repository
```bash
git clone https://github.com/primecoderIN/Shipwise.git
cd Shipwise
```

### 2. Spin up the infrastructure
Run the provided PowerShell script to bootstrap the local environment (PostgreSQL, Redis, Mailpit, MinIO).
```powershell
.\start.ps1
```
*(The script will automatically generate your `.env` file from `.env.example` if it doesn't exist).*

### 3. Access Local Services
Once running, you can access the local infrastructure tools:
- **Mailpit (Email Testing)**: [http://localhost:8025](http://localhost:8025)
- **MinIO (S3 Storage UI)**: [http://localhost:9001](http://localhost:9001)

To cleanly shut down the environment, run `.\stop.ps1`.

---

## 📚 Documentation

For a deeper dive into the architectural decisions, database schema, and API contracts, please refer to the dedicated documentation files:

* [**Domain Vocabulary**](./docs/domain-vocabulary.md) — The Ubiquitous Language used across the Shipwise platform.
* [**Release Lifecycle**](./docs/release-lifecycle.md) — The core state machine defining a Release.
* [**Development Workflow**](./docs/development-workflow.md) — The lifecycle and state machine of engineering Tasks.
* [**Testing Workflow**](./docs/testing-workflow.md) — How QA and test cases are managed.
* [**Approval Workflow**](./docs/approval-workflow.md) — Auditable sign-offs and deployment gates.
* [**Backend Architecture & Patterns**](./docs/architecture.md) — CQRS, MediatR, and project dependency rules.
* [**General Onboarding & Setup**](./docs/ONBOARDING.md) — *(Coming soon)* Quick-start guide and local environment configuration.
* [**Project Folder Structure**](./docs/folder-structure.md) — Complete annotated directory tree demonstrating our Single-Project Module architecture.
* [**Authorization & Security Guide**](./docs/authorization.md) — *(Coming soon)* RBAC matrices, tenant isolation, and BOLA mitigation.
* [**API Endpoints Catalog**](./docs/api-endpoints.md) — *(Coming soon)* Comprehensive list of routes and standard envelopes.
* [**Database Schema & Migrations**](./docs/database-schema.md) — *(Coming soon)* Mapping of schemas, indexes, and migrations.

---

## 📝 License
This project is licensed under the MIT License — see the [LICENSE](./LICENSE) file for details.