using MediatR;

namespace Shipwise.Modules.Identity.Application.Tenants.CreateTenant;

public record CreateTenantCommand(
    string TenantName,
    string UserEmail,
    string UserFirstName,
    string UserLastName,
    string IdentityId) : IRequest<Guid>;
