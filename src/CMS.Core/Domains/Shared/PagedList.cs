using System.Collections.Generic;

namespace CMS.Core.Domains.Shared
{
    public class PagedList<T>
    {
        public int Total { get; }
        public List<T> Items { get; }

        public PagedList(int total, List<T> items)
        {
            Total = total;
            Items = items;
        }

        public static PagedList<T> Empty => new PagedList<T>(0, new List<T>());
    }
}
