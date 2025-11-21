namespace PoCWebApi.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string OwnerOid { get; set; } = "";
        public string Title { get; set; } = "";
        public bool IsDone { get; set; }
    }
}
