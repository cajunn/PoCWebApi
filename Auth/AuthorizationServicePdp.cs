using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PoCWebApi.Auth.Models;
using PoCWebApi.Data;

namespace PoCWebApi.Auth;

public interface IAuthorizationServicePdp
{
    Task<HashSet<string>> GetPermissionsAsync(Guid oid, CancellationToken ct = default);
    Task<bool> HasPermissionAsync(Guid oid, string permissionKey, CancellationToken ct = default);
}

public sealed class AuthorizationServicePdp(AppDbContext db, IMemoryCache cache, ILogger<AuthorizationServicePdp> log)
    : IAuthorizationServicePdp
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(90);

    public async Task<HashSet<string>> GetPermissionsAsync(Guid oid, CancellationToken ct = default)
    {
        var cacheKey = $"pdp:perms:{oid}";
        if (cache.TryGetValue<HashSet<string>>(cacheKey, out var cached))
            return cached!;

        // Ensure user row exists (JIT join on first use)
        var user = await db.Users.FindAsync(new object?[] { oid }, ct);
        if (user is null)
        {
            user = new User { Oid = oid };
            db.Users.Add(user);
            await db.SaveChangesAsync(ct);
        }

        // User -> Roles -> RoleServices -> ServicePermissions -> Permissions.Key
        var perms = await db.UserRoles
            .Where(ur => ur.Oid == oid)
            .SelectMany(ur => ur.Role.RoleServices)
            .SelectMany(rs => rs.Service.ServicePermissions)
            .Select(sp => sp.Permission.Key)
            .Distinct()
            .ToListAsync(ct);

        var set = new HashSet<string>(perms, StringComparer.OrdinalIgnoreCase);
        cache.Set(cacheKey, set, CacheTtl);
        return set;
    }

    public async Task<bool> HasPermissionAsync(Guid oid, string permissionKey, CancellationToken ct = default)
    {
        var set = await GetPermissionsAsync(oid, ct);
        return set.Contains(permissionKey);
    }
}
