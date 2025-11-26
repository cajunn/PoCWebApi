using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PoCWebApi.Auth.Controllers;

[ApiController]
[Route("authz")]
public sealed class AuthzController(IAuthorizationServicePdp pdp) : ControllerBase
{
    [HttpGet("me/permissions")]
    [Authorize] // must have a valid Entra JWT
    public async Task<IActionResult> MePermissions(CancellationToken ct)
    {
        var oidStr = User.FindFirstValue("oid");
        if (!Guid.TryParse(oidStr, out var oid))
            return Forbid();

        var perms = await pdp.GetPermissionsAsync(oid, ct);
        return Ok(perms); // string[]
    }
}