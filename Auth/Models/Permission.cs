namespace PoCWebApi.Auth.Models
{
    public class Permission
    {
        public int PermissionId { get; set; }

        // e.g. "Client.View", "Vehicle.Create", "Permit.Create"
        public string Key { get; set; } = default!;
        public string? Description { get; set; }

        public ICollection<ServicePermission> ServicePermissions { get; set; } = new List<ServicePermission>();
    }
}
