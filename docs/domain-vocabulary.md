# Domain Vocabulary (Ubiquitous Language)

In Shipwise, we strictly adhere to Domain-Driven Design (DDD) principles. This means that the code, the database, and our conversations must use this exact terminology to prevent translation errors between business requirements and technical implementation.

## Core Hierarchy

### 1. Organization (Tenant)
The top-level billing and administrative entity. An Organization represents a single company using Shipwise. All data is strictly isolated at the Organization level (Multi-Tenancy).

### 2. Workspace
A logical grouping within an Organization. Large companies might have multiple Workspaces (e.g., "Mobile App Team", "Backend API Team"). Users are granted roles at the Workspace level.

### 3. Release
The absolute core of Shipwise. A **Release** is an independent, time-bound or feature-bound workspace (e.g., "v1.4.0" or "Q3 Major Update"). It contains its own backlog, tasks, bugs, approvals, and deployments. Shipwise tracks the health and progress of the Release as a whole.

---

## Work Items

### Backlog Item
A high-level requirement or feature request. It lives in the backlog until it is pulled into a specific Release.

### Task
A specific, actionable unit of engineering work assigned to a developer within a Release. Tasks can have subtasks, dependencies, and PRs linked to them.

### Bug
A defect identified in the software. Bugs can be linked to specific Tasks or directly to a Release.

### Deferred Bug
A Bug that has been triaged but explicitly pushed out of the current Release to be handled in a future version. This is critical for release velocity tracking.

---

## Quality & Delivery

### Test Case
A predefined scenario executed by QA or Developers against the Release to verify behavior. It can pass, fail, or be blocked.

### Review
A peer-review process (usually tied to a GitHub/Azure DevOps Pull Request) required before a Task can be marked as complete.

### Approval / Sign-off
A formal, recorded "Go/No-Go" decision made by a stakeholder (e.g., Product Manager, QA Lead) required before a Release can be deployed.

### Deployment
The recorded event of moving a Release into a specific environment (e.g., "Staging", "Production"). Deployments have checklists and can trigger smoke tests or rollbacks.

---

## UI / Customization

### Widget
A dynamic UI component available in the Shipwise Visual Builder. Users can drag and drop Widgets (e.g., "Timeline", "Metric Chart", "Approvals List") onto a canvas to create custom Release Overviews.
