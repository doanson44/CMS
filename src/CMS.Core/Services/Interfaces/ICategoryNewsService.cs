using CMS.Core.Domains;
using CMS.Core.Domains.Shared;
using System.Threading.Tasks;

namespace CMS.Core.Services.Interfaces
{
    public interface ICategoryNewsService
    {
        Task CreateAsync(CategoryNewsRequest request);
        Task DeleteAsync(int id);
        Task<PagedList<CategoryNewsDto>> GetAllAsync(CategoryNewsQueryParam query);
        Task<CategoryNewsDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, CategoryNewsRequest request);
    }
}
