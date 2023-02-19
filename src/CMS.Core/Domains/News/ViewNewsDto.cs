using CMS.Core.Data.Entities;

namespace CMS.Core.Domains
{
    public class ViewNewsDto
    {
        public long Id { get; set; }

        public int Count { get; set; }

        public ViewNewsDto(ViewNews e)
        {
            Id = e.Id;
            Count = e.Count;
        }
    }
}
