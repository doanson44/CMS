using System;
using System.Threading.Tasks;
using CMS.Core.Domains;
using CMS.Core.Domains.Shared;

namespace CMS.Core.Services.Interfaces;

public interface IDetailNewsService
{
    Task CreateAsync(DetailNewsRequest request);
    Task DeleteAsync(Guid id);
    Task<PagedList<DetailNewsDto>> GetAllAsync(DetailNewsQueryParam query);
    Task<DetailNewsDto> GetByIdAsync(Guid id);
    Task UpdateAsync(Guid id, DetailNewsRequest request);
}
