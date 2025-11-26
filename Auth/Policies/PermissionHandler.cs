using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PoCWebApi.Auth.Policies;

public sealed class PermissionHandler(
    IAuthorizationServicePdp pdp,
    ILogger<PermissionHandler> log)
    : AuthorizationHandler<PermissionRequirement>
{
    private const string OidShort = "oid";
    private const string OidUri = "http://schemas.microsoft.com/identity/claims/objectidentifier";

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var principal = (context.Resource as HttpContext)?.User ?? context.User;

        var oidStr =
            principal.FindFirstValue(OidShort) ??
            principal.FindFirstValue(OidUri);

        if (!Guid.TryParse(oidStr, out var oid))
        {
            log.LogWarning("PermissionHandler: could not find/parse OID claim for user.");
            return;
        }

        var allowed = await pdp.HasPermissionAsync(oid, requirement.Permission);

        if (allowed)
        {
            context.Succeed(requirement);
        }
        else
        {
            log.LogInformation("PermissionHandler: user {Oid} does NOT have permission {Permission}", oid, requirement.Permission);
        }
    }
}
