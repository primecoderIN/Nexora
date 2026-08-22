using FluentValidation;

namespace Nexora.Api.Extensions;

public static class ApiServiceExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        // Register Global Exception Handler (.NET 8+ standard)
        services.AddExceptionHandler<Nexora.Api.Middleware.GlobalExceptionHandler>();
        services.AddProblemDetails();
        
        // Register MediatR
        services.AddMediatR(cfg => 
        {
            cfg.RegisterServicesFromAssembly(typeof(Nexora.Modules.Identity.Application.Tenants.CreateTenant.CreateTenantCommand).Assembly);
            cfg.AddOpenBehavior(typeof(Nexora.Shared.Validation.ValidationBehavior<,>));
        });

        // Register FluentValidation
        services.AddValidatorsFromAssembly(typeof(Nexora.Modules.Identity.Application.Tenants.CreateTenant.CreateTenantCommandValidator).Assembly);
        
        return services;
    }
}
