using Microsoft.AspNetCore.Authorization;

namespace PoCWebApi.Authorization
{
    public sealed class PermissionHandler(IPdpClient pdp, IDataScopeAccessor scopeAcc, ILogger<PermissionHandler> log)
        : AuthorizationHandler<PermissionRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var user = (context.Resource as HttpContext)?.User ?? context.User;
            if (user?.Identity?.IsAuthenticated != true)
                return;

            var result = await pdp.EvaluateAsync(user!, requirement.Permission);
            if (!result.Allow) return;

            // Save scope for downstream (repo/EF)
            scopeAcc.Current = new DataScope
            {
                Items = result.Scope is null ? new Dictionary<string, string[]>() : new Dictionary<string, string[]>(result.Scope)
            };
            context.Succeed(requirement);
        }
    }
}
