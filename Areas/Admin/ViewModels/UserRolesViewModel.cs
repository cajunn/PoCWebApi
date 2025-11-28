namespace PoCWebApi.Areas.Admin.ViewModels
{
    public class UserRolesViewModel
    {
        public Guid Oid { get; set; }
        public string? DisplayName { get; set; }
        public string? Email { get; set; }

        public List<RoleCheckboxItem> Roles { get; set; } = new();
    }

    public class RoleCheckboxItem
    {
        public int RoleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsAssigned { get; set; }
    }
}
