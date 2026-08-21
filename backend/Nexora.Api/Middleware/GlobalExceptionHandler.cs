using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nexora.Shared.Exceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nexora.Api.Middleware;

/// <summary>
/// Global exception handler that catches unhandled exceptions and formats them 
/// into standardized Problem Details JSON responses (RFC 7807).
/// This replaces the legacy try/catch middleware pattern in .NET 8+.
/// </summary>
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment env) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // 1. Initialize the standard ProblemDetails object.
        // ProblemDetails is the industry standard (RFC 7807) for returning errors in HTTP APIs.
        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path // Helps trace which endpoint caused the crash
        };

        // 2. Map specific exception types to the correct HTTP Status Code and message.
        switch (exception)
        {
            case ValidationException validationEx:
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Validation Failed";
                problemDetails.Detail = "One or more validation errors occurred.";
                
                // Map FluentValidation errors by property name directly into the ProblemDetails extensions.
                // This allows the Angular frontend to easily highlight the exact form fields that failed.
                problemDetails.Extensions["errors"] = validationEx.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );
                break;

            case NotFoundException notFoundEx:
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Title = "Not Found";
                problemDetails.Detail = notFoundEx.Message;
                break;
                
            case BusinessRuleException businessRuleEx:
                // 409 Conflict is ideal for when a domain rule is violated (e.g., "Cannot delete an active project")
                problemDetails.Status = StatusCodes.Status409Conflict;
                problemDetails.Title = "Business Rule Violation";
                problemDetails.Detail = businessRuleEx.Message;
                break;

            case UnauthorizedException unauthorizedEx:
                // 401 means the user needs to log in
                problemDetails.Status = StatusCodes.Status401Unauthorized;
                problemDetails.Title = "Unauthorized";
                problemDetails.Detail = unauthorizedEx.Message;
                break;

            case ForbiddenAccessException forbiddenEx:
                // 403 means the user is logged in, but doesn't have permission for this specific action
                problemDetails.Status = StatusCodes.Status403Forbidden;
                problemDetails.Title = "Forbidden Access";
                problemDetails.Detail = forbiddenEx.Message;
                break;

            case DomainException domainEx:
                // Generic fallback for any other domain-related exceptions
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Domain Exception";
                problemDetails.Detail = domainEx.Message;
                break;
                
            default:
                // If it's a completely unexpected exception (e.g., a database connection failure or NullReferenceException)
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Title = "Internal Server Error";
                
                // SECURITY FEATURE: 
                // Only surface the real exception message if we are running in Development.
                // This prevents sensitive stack traces or database schema names from leaking to hackers in Production.
                problemDetails.Detail = env.IsDevelopment() 
                    ? exception.Message 
                    : "An unexpected error occurred while processing your request. Please try again later.";
                break;
        }

        // 3. Log the error.
        // We only want to log a true "Error" (which might trigger a PagerDuty alert) if the status code is 500+.
        // If it's just a user making a typo (400 Bad Request) or trying to access something they shouldn't (403), 
        // a "Warning" is much more appropriate.
        if (problemDetails.Status >= 500)
        {
            logger.LogError(exception, "Server Error: {Message}", exception.Message);
        }
        else
        {
            logger.LogWarning("Client Error ({StatusCode}): {Message}", problemDetails.Status, exception.Message);
        }

        // 4. Write the ProblemDetails JSON to the HTTP response.
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        // 5. Return true to tell ASP.NET Core that this exception has been fully handled 
        // and shouldn't crash the server.
        return true;
    }
}
