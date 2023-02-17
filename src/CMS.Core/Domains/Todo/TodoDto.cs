using CMS.Core.Data.Entities.Todo;

namespace CMS.Core.Domains
{
    public class TodoDto
    {
        public int TodoId { get; set; }

        public string Item { get; set; }
        public int Minutes { get; set; }
        public string Description { get; set; }

        public TodoDto(TodoItem e)
        {
            TodoId = e.TodoItemId;
            Item = e.Item;
            Minutes = e.Minutes;
            Description = e.Description;
        }
    }
}
