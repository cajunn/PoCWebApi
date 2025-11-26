namespace PoCWebApi.Auth.Models
{
    public class UserRole
    {
        public Guid Oid { get; set; }
        public User User { get; set; } = default!;

        public int RoleId { get; set; }
        public Role Role { get; set; } = default!;
    }
}
