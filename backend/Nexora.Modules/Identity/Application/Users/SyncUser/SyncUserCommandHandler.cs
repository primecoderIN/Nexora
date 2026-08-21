using MediatR;
using Microsoft.EntityFrameworkCore;
using Nexora.Modules.Identity.Domain.Entities;
using Nexora.Modules.Identity.Persistence;

namespace Nexora.Modules.Identity.Application.Users.SyncUser;

public class SyncUserCommandHandler(IdentityDbContext dbContext) : IRequestHandler<SyncUserCommand, Guid>
{
    public async Task<Guid> Handle(SyncUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Ensure the Tenant exists
        var tenantExists = await dbContext.Tenants
            .AnyAsync(t => t.Id == request.TenantId, cancellationToken);
            
        if (!tenantExists)
        {
            throw new InvalidOperationException($"Tenant with ID {request.TenantId} does not exist.");
        }

        // 2. Check if user already exists
        var existingUser = await dbContext.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.IdentityId == request.IdentityId, cancellationToken);

        if (existingUser != null)
        {
            // If they exist, they should ideally be part of the requested Tenant.
            // If we support users in multiple tenants later, we would handle TenantUser records here.
            // For now, since User has a strict 1:1 with TenantId, we just verify it matches or return it.
            if (existingUser.TenantId != request.TenantId)
            {
                throw new InvalidOperationException("User already belongs to a different organization.");
            }
            
            return existingUser.Id;
        }

        // 3. Create the User and assign them to the requested Tenant
        var userId = Guid.NewGuid();
        var user = new User(
            id: userId,
            identityId: request.IdentityId,
            email: request.UserEmail,
            firstName: request.UserFirstName,
            lastName: request.UserLastName,
            tenantId: request.TenantId
        );

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return userId;
    }
}
