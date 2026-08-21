using MediatR;

namespace Nexora.Modules.Identity.Application.Users.SyncUser;

public record SyncUserCommand(
    string UserEmail,
    string UserFirstName,
    string UserLastName,
    string IdentityId,
    Guid TenantId) : IRequest<Guid>;
