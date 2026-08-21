# Release Lifecycle

In Nexora, the **Release** is the primary container for work. Understanding the state machine of a Release is critical for both the frontend UI (which buttons are enabled/disabled) and the backend API (validation rules for state transitions).

## The State Machine

A Release strictly follows this lifecycle:

1. **Planning (Draft)**
   - **Trigger**: A new Release is created.
   - **Allowed Actions**: Add Backlog Items, define Team members, set target dates.
   - **Constraint**: Development cannot start until the Release is formally moved to "In Progress".

2. **In Progress (Active Development)**
   - **Trigger**: Project Manager clicks "Start Release".
   - **Allowed Actions**: Developers can claim Tasks, open PRs, move Tasks to "Review".
   - **Constraint**: The Release cannot move to Testing if there are blocked Tasks.

3. **Testing (Code Freeze)**
   - **Trigger**: Development is finalized; all Tasks are either Done or explicitly Deferred.
   - **Allowed Actions**: QA executes Test Cases, logs Bugs.
   - **Constraint**: Code changes are locked unless a critical Bug requires a hotfix PR.

4. **Approval (Sign-off)**
   - **Trigger**: QA signs off on all Test Cases.
   - **Allowed Actions**: Stakeholders review the Release Summary (metrics, deferred bugs) and provide cryptographic or audited Sign-off.
   - **Constraint**: Cannot deploy to Production without required Approvals.

5. **Deploying (Staging & Production)**
   - **Trigger**: Approvals are complete.
   - **Allowed Actions**: Deployment checklists are executed.
   - **Constraint**: If deployment fails, the state reverts or moves to a "Rollback" state.

6. **Live (Monitoring)**
   - **Trigger**: Production deployment is marked successful.
   - **Allowed Actions**: Post-go-live metrics are gathered. Hotfixes can be linked back to this Release.

7. **Closed (Archived)**
   - **Trigger**: The monitoring period expires without critical issues.
   - **Allowed Actions**: Read-only historical data.
