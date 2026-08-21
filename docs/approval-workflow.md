# Approval Workflow

In Nexora, the **Approval (Sign-off)** phase acts as the final gatekeeper before a Release is deployed to Production. This workflow ensures compliance, accountability, and security.

## The Approval Model

Approvals are not just simple checkboxes; they are formal, auditable records linked to specific roles or users.

### 1. Approval Definition
Before a Release enters the "Approval" state, the necessary approvers must be defined. This can be:
- **Role-based**: e.g., "At least one user with the 'QA Lead' role must approve."
- **User-based**: e.g., "Alice and Bob must explicitly approve."

### 2. State Machine for Approvals

When the Release transitions to the **Approval (Sign-off)** phase, individual approval requests are generated. Each request follows this lifecycle:

1. **Pending**
   - The approval request is sent to the designated user or role group.
   - The user is notified (in-app and via email/Slack).

2. **Approved**
   - The stakeholder reviews the Release Summary (metrics, passed tests, deferred bugs).
   - They formally approve.
   - *Note: In a high-compliance environment, this may require re-authenticating or providing a cryptographic signature.*

3. **Rejected**
   - The stakeholder identifies a critical issue and rejects the release.
   - **Trigger**: Rejection requires a mandatory comment or linking to a blocking Bug.
   - A single rejection typically blocks the entire Release from proceeding to Deployment.

## Deployment Gate
The backend API enforces a strict gate: A Deployment entity cannot be created or executed unless all required Approvals for the target environment are in the **Approved** state.
