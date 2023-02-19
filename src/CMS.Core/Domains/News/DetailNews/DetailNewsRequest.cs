using CMS.Core.Enums;
using System;

namespace CMS.Core.Domains
{
    public class DetailNewsRequest
    {
        public Guid? Id { get; set; }

        public int? CategoryNewsId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime ExpiredDate { get; set; }

        public DetailNewsStatus Status { get; set; }
    }
}
