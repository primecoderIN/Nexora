using FluentValidation;

namespace Nexora.Modules.Identity.Application.Users.SyncUser;

public class SyncUserCommandValidator : AbstractValidator<SyncUserCommand>
{
    public SyncUserCommandValidator()
    {
        RuleFor(x => x.UserEmail).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.UserFirstName).NotEmpty().MaximumLength(128);
        RuleFor(x => x.UserLastName).NotEmpty().MaximumLength(128);
        RuleFor(x => x.IdentityId).NotEmpty().MaximumLength(256);
        RuleFor(x => x.TenantId).NotEmpty().WithMessage("A valid Tenant ID is required to sync a user.");
    }
}
