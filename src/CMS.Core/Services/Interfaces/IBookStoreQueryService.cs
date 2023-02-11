using CMS.Core.Domains;

namespace CMS.Core.Services.Interfaces
{
    public interface IBookStoreQueryService
    {
        public BookSPDto GetBookStoreFromStoredProcedure(long bookId);
    }
}
