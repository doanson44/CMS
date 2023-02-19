using CMS.Core.Domains.Shared;
using CMS.Core.Enums;

namespace CMS.Core.Domains
{
    public class DetailNewsQueryParam : QueryParam
    {
        public string CurrentUser { get; set; }

        public DetailNewsStatus? Status { get; set; }

        public int? CategoryNewsId { get; set; }
    }
}
