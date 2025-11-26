namespace PoCWebApi.Auth.Models
{
    public class Service
    {
        public int ServiceId { get; set; }

        // e.g. "RegisterVehicle", "IssuePermit"
        public string Key { get; set; } = default!;
        public string? Description { get; set; }

        public ICollection<RoleService> RoleServices { get; set; } = new List<RoleService>();
        public ICollection<ServicePermission> ServicePermissions { get; set; } = new List<ServicePermission>();
    }
}
