using CMS.Core.Domains;

namespace CMS.Core.Data.Entities.Todo
{
    public class TodoItem : BaseEntity
    {
        public int TodoItemId { get; set; }
        public string Item { get; set; }
        public int Minutes { get; set; }
        public string Description { get; set; }

        public TodoItem()
        {

        }

        public TodoItem(TodoRequest request)
        {
            TodoItemId = request.Id ?? 0;
            Item = request.Item;
            Minutes = request.Minutes;
            Description = request.Description;
        }
    }
}
