using FluentValidation;

namespace Nexora.Modules.Identity.Application.Tenants.CreateTenant;

public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(x => x.TenantName).NotEmpty().MaximumLength(256);
        RuleFor(x => x.UserEmail).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.UserFirstName).NotEmpty().MaximumLength(128);
        RuleFor(x => x.UserLastName).NotEmpty().MaximumLength(128);
        RuleFor(x => x.IdentityId).NotEmpty().MaximumLength(256);
    }
}
