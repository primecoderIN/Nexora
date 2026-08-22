using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Nexora.Api.Extensions;

/// <summary>
/// Responsibility: Configures services strictly belonging to the Application and Domain logic layer.
/// This includes registering CQRS handlers (MediatR) and validation pipelines (FluentValidation).
/// </summary>
public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // FluentValidation: Scans the specified project assemblies and registers all Validation classes it finds.
        // Because this is a Modular Monolith, we have to tell it to scan each module separately.
        services.AddValidatorsFromAssemblyContaining<Nexora.Modules.Identity.Application.Tenants.CreateTenant.CreateTenantCommandValidator>();
        
        // Configures MediatR, the library we use to implement the CQRS (Command Query Responsibility Segregation) pattern.
        services.AddMediatR(cfg => 
        {
            // Tells MediatR to scan these specific module assemblies and register all Command/Query Handlers it finds.
            cfg.RegisterServicesFromAssemblyContaining<Nexora.Modules.Identity.Application.Tenants.CreateTenant.CreateTenantCommand>();
            
            // MediatR Pipeline Behavior (Middleware): MediatR and FluentValidation are two completely separate libraries.
            // MediatR knows nothing about the validators we registered above. This "ValidationBehavior" acts as a bridge.
            // It tells MediatR: "Before executing any handler, check the DI container for any FluentValidation rules that match this command, and run them first."
            cfg.AddOpenBehavior(typeof(Nexora.Shared.Validation.ValidationBehavior<,>));
        });

        return services;
    }
}
