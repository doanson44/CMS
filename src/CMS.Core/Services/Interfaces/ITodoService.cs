using System.Threading.Tasks;
using CMS.Core.Domains;
using CMS.Core.Domains.Shared;

namespace CMS.Core.Services.Interfaces;

public interface ITodoService
{
    Task CreateAsync(TodoRequest request);
    Task DeleteAsync(int id);
    Task<PagedList<TodoDto>> GetAllAsync(TodoQueryParam query);
    Task<TodoDto> GetByIdAsync(int id);
    Task UpdateAsync(int id, TodoRequest request);
}
