namespace PoCWebApi.Auth.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<RoleService> RoleServices { get; set; } = new List<RoleService>();
    }
}
