namespace PoCWebApi.Auth.Models
{
    public class ServicePermission
    {
        public int ServiceId { get; set; }
        public Service Service { get; set; } = default!;

        public int PermissionId { get; set; }
        public Permission Permission { get; set; } = default!;
    }
}
