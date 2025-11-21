using Microsoft.AspNetCore.Authorization;

namespace PoCWebApi.Authorization
{
    public sealed class PermissionRequirement(string permission) : IAuthorizationRequirement
    {
        public string Permission { get; } = permission;
    }
}
