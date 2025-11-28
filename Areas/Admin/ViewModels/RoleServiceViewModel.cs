namespace PoCWebApi.Areas.Admin.ViewModels
{
    public class RoleServicesViewModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;

        public List<ServiceCheckboxItem> Services { get; set; } = new();
    }

    public class ServiceCheckboxItem
    {
        public int ServiceId { get; set; }
        public string Key { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsAssigned { get; set; }
    }
}
