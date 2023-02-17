namespace CMS.Core.Domains
{
    public class TodoRequest
    {
        public int? Id { get; set; }
        public string Item { get; set; }
        public int Minutes { get; set; }
        public string Description { get; set; }
    }
}
