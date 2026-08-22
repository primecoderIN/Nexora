using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Nexora.Shared.Interfaces;
using Nexora.Modules.Identity.API.Services;

namespace Nexora.Api.Extensions;

/// <summary>
/// Responsibility: Configures services strictly related to Identity, Authentication, and Authorization.
/// This includes extracting the current user context and validating JWT tokens against our OIDC provider (Keycloak).
/// </summary>
public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Registers IHttpContextAccessor so that we can read headers and claims from the incoming HTTP request.
        services.AddHttpContextAccessor();
        
        // Registers our abstraction for the current user. When the application layer needs to know who is logged in
        // or their tenant, it asks for ICurrentUserContext, and the DI container provides CurrentUserContext.
        services.AddScoped<ICurrentUserContext, CurrentUserContext>();

        // Registers JWT Bearer authentication scheme.
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // These settings point to our local Keycloak docker container (OIDC Provider).
                // Authority is where the API will fetch the public JWKS keys to verify token signatures.
                options.Authority = configuration["Authentication:Keycloak:Authority"] ?? "http://localhost:8080/realms/nexora";
                
                // Allow non-HTTPS for local development inside Docker network.
                options.RequireHttpsMetadata = false; 
                
                // Ensure the token was specifically minted for our API.
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
