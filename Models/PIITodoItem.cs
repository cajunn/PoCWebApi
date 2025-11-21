namespace PoCWebApi.Models
{
    public class PIITodoItem
    {
        public int Id { get; set; }
        public string OwnerOid { get; set; } = "";
        public string PIITitle { get; set; } = "";
        public bool IsDone { get; set; }
    }
}
