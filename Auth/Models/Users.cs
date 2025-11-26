namespace PoCWebApi.Auth.Models
{
    public class User
    {
        // Entra object id (oid claim) – primary key
        public Guid Oid { get; set; }

        public string? DisplayName { get; set; }
        public string? Email { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
