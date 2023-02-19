using CMS.Core.Domains;
using CMS.Core.Domains.Shared;
using System;
using System.Threading.Tasks;

namespace CMS.Core.Services.Interfaces
{
    public interface IDetailNewsService
    {
        Task CreateAsync(DetailNewsRequest request);
        Task DeleteAsync(Guid id);
        Task<PagedList<DetailNewsDto>> GetAllAsync(DetailNewsQueryParam query);
        Task<DetailNewsDto> GetByIdAsync(Guid id);
        Task UpdateAsync(Guid id, DetailNewsRequest request);
    }
}
