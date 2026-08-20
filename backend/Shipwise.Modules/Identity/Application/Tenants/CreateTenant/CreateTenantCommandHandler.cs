using MediatR;
using Microsoft.EntityFrameworkCore;
using Shipwise.Modules.Identity.Domain.Entities;
using Shipwise.Modules.Identity.Persistence;

namespace Shipwise.Modules.Identity.Application.Tenants.CreateTenant;

public class CreateTenantCommandHandler(IdentityDbContext dbContext) : IRequestHandler<CreateTenantCommand, Guid>
{
    public async Task<Guid> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        // Check if user already exists
        var userExists = await dbContext.Users
            .IgnoreQueryFilters()
            .AnyAsync(u => u.IdentityId == request.IdentityId, cancellationToken);

        if (userExists)
        {
            throw new InvalidOperationException("User already belongs to an organization.");
        }

        // Create the Tenant
        var tenantId = Guid.NewGuid();
        var tenant = new Tenant(tenantId, request.TenantName);

        // Create the Owner User
        var userId = Guid.NewGuid();
        var user = new User(
            id: userId,
            identityId: request.IdentityId,
            email: request.UserEmail,
            firstName: request.UserFirstName,
            lastName: request.UserLastName,
            tenantId: tenantId
        );

        dbContext.Tenants.Add(tenant);
        dbContext.Users.Add(user);

        await dbContext.SaveChangesAsync(cancellationToken);

        return tenantId;
    }
}
