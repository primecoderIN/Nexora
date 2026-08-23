using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexora.Modules.Identity.Application.Users.SyncUser;

namespace Nexora.Modules.Identity.API.Controllers;

/// <summary>
/// API Controller for managing User synchronization.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController(IMediator mediator) : ControllerBase
{
    public record SyncUserRequest(Guid TenantId);

    /// <summary>
    /// Synchronizes an authenticated user from Keycloak into the Nexora database, 
    /// associating them with the specified Tenant.
    /// </summary>
    /// <param name="request">The request containing the Target Tenant ID.</param>
    /// <returns>The synchronized User ID.</returns>

    [HttpPost("sync")]
    public async Task<IActionResult> SyncUser([FromBody] SyncUserRequest request)
    {
        var identityId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value ?? User.FindFirst("email")?.Value;
        var firstName = User.FindFirst(ClaimTypes.GivenName)?.Value ?? User.FindFirst("given_name")?.Value ?? "";
        var lastName = User.FindFirst(ClaimTypes.Surname)?.Value ?? User.FindFirst("family_name")?.Value ?? "";

        if (string.IsNullOrEmpty(identityId) || string.IsNullOrEmpty(email))
        {
            return BadRequest("Invalid JWT token: Missing sub or email claims.");
        }

        var command = new SyncUserCommand(
            email,
            firstName,
            lastName,
            identityId,
            request.TenantId
        );

        var userId = await mediator.Send(command);
        return Ok(new { UserId = userId });
    }
}
