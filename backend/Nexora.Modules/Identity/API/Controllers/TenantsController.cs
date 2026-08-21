using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexora.Modules.Identity.Application.Tenants.CreateTenant;

namespace Nexora.Modules.Identity.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TenantsController(IMediator mediator) : ControllerBase
{
    public record CreateTenantRequest(string TenantName);

    [HttpPost]
    public async Task<IActionResult> CreateTenant([FromBody] CreateTenantRequest request)
    {
        var identityId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value ?? User.FindFirst("email")?.Value;
        var firstName = User.FindFirst(ClaimTypes.GivenName)?.Value ?? User.FindFirst("given_name")?.Value ?? "";
        var lastName = User.FindFirst(ClaimTypes.Surname)?.Value ?? User.FindFirst("family_name")?.Value ?? "";

        if (string.IsNullOrEmpty(identityId) || string.IsNullOrEmpty(email))
        {
            return BadRequest("Invalid JWT token: Missing sub or email claims.");
        }

        var command = new CreateTenantCommand(
            request.TenantName,
            email,
            firstName,
            lastName,
            identityId
        );

        var tenantId = await mediator.Send(command);
        return Ok(new { TenantId = tenantId });
    }
}
