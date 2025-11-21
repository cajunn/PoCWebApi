using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace PoCWebApi.Authorization
{
    public sealed class DynamicPermissionPolicyProvider(IOptions<AuthorizationOptions> fallback) : IAuthorizationPolicyProvider
    {
        private const string Prefix = "perm:";

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase))
            {
                var permission = policyName.Substring(Prefix.Length);
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
}
