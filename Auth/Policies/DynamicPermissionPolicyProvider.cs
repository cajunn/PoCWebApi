using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace PoCWebApi.Auth.Policies;

public sealed class DynamicPermissionPolicyProvider(IOptions<AuthorizationOptions> fallback)
    : IAuthorizationPolicyProvider
{
    private const string Prefix = "perm:";

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase))
        {
            var permission = policyName[Prefix.Length..]; // strip "perm:"
            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(permission))
                .Build();
            return Task.FromResult<AuthorizationPolicy?>(policy);
        }

        return Task.FromResult<AuthorizationPolicy?>(null);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => Task.FromResult(fallback.Value.DefaultPolicy);
    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => Task.FromResult(fallback.Value.FallbackPolicy);
}