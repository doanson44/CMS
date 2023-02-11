namespace CMS.Core.Data.Specifications
{
    public class SpecificationParam
    {
        public int Page { get; private set; }
        public int Take { get; private set; }
        public string Search { get; private set; }
        public string Sort { get; private set; }
        public bool Asc { get; private set; }

        public SpecificationParam(int page, int take, string search, string sort, bool asc)
        {
            Page = page;
            Take = take;
            Search = string.IsNullOrWhiteSpace(search) ? string.Empty : search.ToLower().Trim();
            Sort = string.IsNullOrWhiteSpace(sort) ? string.Empty : sort.ToLower();
            Asc = asc;
        }

        public (int page, int take, string search, string sort, bool asc) Params =>
            (Page, Take, Search, Sort, Asc);
    }
}
