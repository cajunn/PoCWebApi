namespace PoCWebApi.Auth.Models
{
    public class RoleService
    {
        public int RoleId { get; set; }
        public Role Role { get; set; } = default!;

        public int ServiceId { get; set; }
        public Service Service { get; set; } = default!;
    }
}
