using CMS.Core.Data.Specifications;
using CMS.Core.Domains.Shared;

namespace CMS.Core.Extensions
{
    public static class SpecificationExtension
    {
        public static SpecificationParam ToSpecificationParam(this QueryParam query)
        {
            return new SpecificationParam(
                query.Page,
                query.Take,
                query.Search?.ToLower()?.Trim(),
                query.SortBy?.ToLower()?.Trim(),
                query.SortDirection?.ToLower() == "asc");
        }
    }
}
