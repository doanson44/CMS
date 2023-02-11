namespace CMS.Core.Data.Entities.Todo
{
    public class TodoItem : BaseEntity<int>
    {
        public string Item { get; set; }
        public int Minutes { get; set; }
        public string Description { get; set; }
    }
}
