# Development Workflow

The Development Workflow in Shipwise defines how individual **Tasks** move from inception to completion within an active Release. This workflow ensures quality control, peer review, and traceability.

## Task State Machine

Every task within a Release follows this lifecycle:

1. **To Do (Unassigned / Backlog)**
   - The task has been scoped for the current Release but development hasn't started.
   - Anyone in the workspace can assign it to a developer or themselves.

2. **In Progress (Active Coding)**
   - The developer claims the task.
   - Typically maps to a branch creation in the connected VCS (e.g., GitHub, Azure DevOps).
   - Expected activity: Commits are being pushed.

3. **In Review (Pull Request)**
   - The developer completes the work and opens a Pull Request.
   - The task automatically shifts to "In Review".
   - Reviewers are notified.

4. **Changes Requested (Feedback Loop)**
   - Reviewers review the code and request changes.
   - The task bounces back to the developer.

5. **Approved**
   - Required reviewers have signed off on the code.
   - The PR is ready to be merged.

6. **Done (Merged & Verified)**
   - The code is merged into the main branch.
   - The task is locked and considered complete for this Release.

## Integrations
Shipwise relies heavily on webhooks from Version Control Systems (GitHub, GitLab, Azure DevOps) to automatically transition these states. For instance, linking a branch name like `feat/SW-102-user-auth` to a task will automatically track commits and PR statuses.
