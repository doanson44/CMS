using System;
using System.Linq;
using System.Threading.Tasks;
using CMS.Core.Data;
using CMS.Core.Data.Entities;
using CMS.Core.Data.Extensions;
using CMS.Core.Data.Repositories;
using CMS.Core.Domains;
using CMS.Core.Domains.Shared;
using CMS.Core.Enums;
using CMS.Core.Exceptions;
using CMS.Core.Services.Interfaces;

namespace CMS.Core.Services;

public class CategoryNewsService : ICategoryNewsService
{
    private readonly IRepository<CategoryNews> _categoryNewsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryNewsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _categoryNewsRepository = unitOfWork.Get<CategoryNews>();
    }

    public async Task<PagedList<CategoryNewsDto>> GetAllAsync(CategoryNewsQueryParam query)
    {
        var (page, take, search, sort, asc) = query.Params;
        var condition = PredicateBuilder.True<CategoryNews>();

        if (!string.IsNullOrWhiteSpace(search))
        {
            condition = condition.And(x => x.Name.Contains(search));
        }

        var result = await _categoryNewsRepository.GetPagedListAsync(
            condition,
            x => new CategoryNewsDto(x),
            o => o.Sort(x => x.Name, true),
            query.Page,
            query.Take);

        return result;
    }

    public async Task CreateAsync(CategoryNewsRequest request)
    {
        var categoryNews = new CategoryNews(request);
        // Check valid duplicate
        var isExist = await _categoryNewsRepository.IsLiveAsync(x => x.Name == categoryNews.Name);
        if (isExist)
        {
            throw new BusinessException(ErrorCodes.CategoryNewsExist);
        }
        await _categoryNewsRepository.AddAsync(categoryNews);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var categoryNews = await _categoryNewsRepository.GetByIdAsync(id, new[] { "DetailNews" });

        if (categoryNews == null)
        {
            throw new BusinessException(ErrorCodes.CategoryNewsNotFound);
        }

        if (categoryNews.DetailNews != null && categoryNews.DetailNews.Any())
        {
            throw new BusinessException(ErrorCodes.NotPermissionDeleteCategoryNews);
        }

        _categoryNewsRepository.Remove(categoryNews);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<CategoryNewsDto> GetByIdAsync(int id)
    {
        var categoryNews = await _categoryNewsRepository.GetByIdAsync(id, new[] { nameof(DetailNews) });
        if (categoryNews == null)
        {
            throw new BusinessException(ErrorCodes.CategoryNewsNotFound);
        }

        var CategoryNewsDto = new CategoryNewsDto(categoryNews);
        return CategoryNewsDto;
    }

    public async Task UpdateAsync(int id, CategoryNewsRequest request)
    {
        var categoryNews = await _categoryNewsRepository.GetByIdAsync(id);
        if (categoryNews == null)
        {
            throw new BusinessException(ErrorCodes.CategoryNewsNotFound);
        }

        var isExist = await _categoryNewsRepository.IsLiveAsync(x => x.Id != categoryNews.Id && x.Name == categoryNews.Name);

        if (isExist)
        {
            throw new BusinessException(ErrorCodes.CategoryNewsExist);
        }

        categoryNews.Name = request.Name;

        _categoryNewsRepository.Update(categoryNews);
        await _unitOfWork.SaveChangesAsync();
    }
}
