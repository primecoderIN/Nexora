# Coding Standards & Enterprise Scale Rule

## Core Directive
When writing code for Shipwise, you must ALWAYS assume the application is operating at an extreme enterprise scale, serving **crores (tens of millions) of requests daily**. Every line of code must be heavily optimized for high concurrency, security, and reusability. 

You must **always use industry best practices** for every single tool we use, every architectural decision we make, and every piece of code we write. There are no shortcuts.

## Instructions
1. **Performance First**: 
   - Never write blocking code (always use `async`/`await`).
   - Optimize database queries (use projection, avoid N+1 queries, use compiled queries where appropriate, and leverage Redis caching for hot paths).
   - Minimize allocations to reduce Garbage Collection pressure (use `Span<T>`, `Memory<T>`, `readonly struct`, etc., where beneficial).
2. **Security by Default**:
   - Validate every input strictly using FluentValidation.
   - Enforce resource-level authorization to prevent **BOLA / IDOR** (Broken Object Level Authorization).
   - Enforce strict role-based and permission-based checks on every endpoint to prevent **BFLA** (Broken Function Level Authorization).
   - Never log sensitive data (PII).
3. **Clean & Reusable**:
   - Strictly adhere to Clean Architecture and SOLID principles.
   - Keep controllers thin and push all business logic into the Application (MediatR Handlers) or Domain (Entities/Services) layers.
   - Avoid code duplication; extract generic patterns into the `Shipwise.Shared` kernel.
4. **Resiliency**:
   - Code must be written expecting failure. Implement retries, circuit breakers, and defensive programming when interacting with external services or the database.
