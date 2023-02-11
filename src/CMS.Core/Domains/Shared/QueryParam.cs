namespace CMS.Core.Domains.Shared
{
    public class QueryParam
    {
        public string Search { get; set; } = string.Empty;
        public int Page { get; set; } = 1;
        public int Take { get; set; } = 10;
        public string SortBy { get; set; } = string.Empty;
        public string SortDirection { get; set; } = string.Empty;

        public bool SortAsc => string.IsNullOrWhiteSpace(SortDirection) || SortDirection == "asc";

        public static QueryParam Default => new QueryParam { Page = 1, Take = 10, Search = string.Empty, SortBy = string.Empty };

        public (int page, int take, string search, string sort, bool asc) Params => (
            Page,
            Take,
            Search?.Trim()?.ToLower() ?? string.Empty,
            SortBy?.Trim()?.ToLower() ?? string.Empty,
            SortAsc);
    }
}
