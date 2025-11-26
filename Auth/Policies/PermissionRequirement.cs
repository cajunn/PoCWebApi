using Microsoft.AspNetCore.Authorization;

namespace PoCWebApi.Auth.Policies;

public sealed class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}