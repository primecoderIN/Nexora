using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Nexora.Shared.Interfaces;
using Nexora.Modules.Identity.API.Services;

namespace Nexora.Api.Extensions;

public static class ApiServiceExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        
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

    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserContext, CurrentUserContext>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // These settings point to our local Keycloak docker container
                options.Authority = configuration["Authentication:Keycloak:Authority"] ?? "http://localhost:8080/realms/nexora";
                options.RequireHttpsMetadata = false; // Because we are running locally over HTTP
                options.Audience = configuration["Authentication:Keycloak:Audience"] ?? "account";
                
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

        return services;
    }
}
