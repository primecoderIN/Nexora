# Documentation Enforcement Rule

## Core Directive
As the AI coding assistant for the Nexora project, you MUST keep all Markdown documentation files up to date with every structural, architectural, or significant code change we make.

## Instructions
1. **Continuous Updates**: Whenever a new module is added, a folder is renamed, or an architectural pattern shifts, you must immediately cross-reference `README.md`, `NEXORA_IMPLEMENTATION_PLAN.md`, and any files in the `docs/` or `knowledge-base/` directories and update them accordingly.
2. **Never Stale**: Do not allow the `docs/` folder to contain outdated information. If a decision is changed during a conversation, proactively update the relevant `.md` files without the user explicitly prompting you.
3. **Phase Documentation**: At the end of every Phase, ensure `knowledge-base/phases/phase-XXX.md` accurately reflects the exact state of the project, the commands used, and the architectural rationale.
