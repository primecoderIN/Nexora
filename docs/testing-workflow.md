# Testing Workflow

Quality Assurance (QA) is a first-class citizen in Shipwise. Testing is conducted within the context of a Release, ensuring that before any sign-off occurs, all targeted scenarios are validated.

## Test Case State Machine

A **Test Case** in Shipwise represents a specific scenario to be verified. It follows this lifecycle:

1. **Draft / Unassigned**
   - The test case has been defined for the Release.
   - It is waiting for a tester to claim it or be assigned.

2. **Ready for Test**
   - The developer has completed the associated Task, or the Release has entered the "Testing" (Code Freeze) phase.
   - The tester is notified.

3. **In Progress**
   - The tester is actively walking through the steps of the Test Case in the staging/testing environment.

4. **Passed**
   - The software behaves as expected.
   - The test case is locked for this release.

5. **Failed**
   - The software did not behave as expected.
   - **Trigger**: Moving a Test Case to "Failed" prompts the tester to log a **Bug**, which is immediately linked to this Test Case and the parent Release.

6. **Blocked**
   - The tester cannot proceed (e.g., the staging database is down, or a third-party API is unreachable).
   - This prevents Release Sign-off until unblocked.

## Bug Lifecycle integration
When a test fails, the resulting Bug follows its own workflow:
* **Triage**: Is it a critical blocker?
* **In Progress**: Developer fixes the bug.
* **Review/Done**: Bug is resolved.
* *Or...* **Deferred**: The stakeholder decides the bug is not critical for this release and pushes it to a future release backlog.
