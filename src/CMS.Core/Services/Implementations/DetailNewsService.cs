using CMS.Core.Data;
using CMS.Core.Data.Entities;
using CMS.Core.Data.Extensions;
using CMS.Core.Data.Repositories;
using CMS.Core.Domains;
using CMS.Core.Domains.Shared;
using CMS.Core.Enums;
using CMS.Core.Exceptions;
using CMS.Core.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace CMS.Core.Services.Implementations
{
    public class DetailNewsService : IDetailNewsService
    {
        private readonly IRepository<DetailNews> detailNewsRepository;
        private readonly IRepository<ViewNews> viewNewsRepository;
        private readonly IRepository<CategoryNews> categoryNewsRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICategoryNewsService categoryNewsService;
        private const string OtherType = "Khác";

        public DetailNewsService(
            IUnitOfWork unitOfWork,
            ICategoryNewsService categoryNewsService)
        {
            this.unitOfWork = unitOfWork;
            this.categoryNewsService = categoryNewsService;
            detailNewsRepository = unitOfWork.Get<DetailNews>();
            viewNewsRepository = unitOfWork.Get<ViewNews>();
            categoryNewsRepository = unitOfWork.Get<CategoryNews>();
        }

        public async Task<PagedList<DetailNewsDto>> GetAllAsync(DetailNewsQueryParam query)
        {
            var (page, take, search, sort, asc) = query.Params;
            var condition = PredicateBuilder.True<DetailNews>();

            if (!string.IsNullOrWhiteSpace(search))
            {
                condition = condition.And(x => x.Title.Contains(search));
            }

            if (query.Status != null)
            {
                condition = condition.And(x => x.Status == query.Status);
            }

            if (query.CategoryNewsId != null)
            {
                condition = condition.And(x => x.CategoryNews.Id == query.CategoryNewsId);
            }

            var result = await detailNewsRepository.GetPagedListAsync(
                condition,
                x => new DetailNewsDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    ExpiredDate = x.ExpiredDate,
                    Status = x.Status,
                    ViewNews = new ViewNewsDto(x.ViewNews),
                    CategoryNews = new CategoryNewsDto(x.CategoryNews)
                },
                o => o.Sort(x => x.Title, true),
                query.Page,
                query.Take,
                new string[] { nameof(ViewNews), nameof(CategoryNews) });

            return result;
        }

        public async Task CreateAsync(DetailNewsRequest request)
        {
            var category = await categoryNewsRepository.GetByIdAsync(request.CategoryNewsId);
            var detailNews = new DetailNews(request);
            detailNews.CategoryNews = category;
            await detailNewsRepository.AddAsync(detailNews);
            var isSuccess = await unitOfWork.SaveChangesAsync();

            if (isSuccess > 0)
            {
                await viewNewsRepository.AddAsync(new ViewNews
                {
                    DetailNewsId = detailNews.Id,
                    Count = 0
                });

                await unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<DetailNewsDto> GetByIdAsync(Guid id)
        {
            var detailNews = await detailNewsRepository.GetByIdAsync(id, new string[] { nameof(ViewNews), nameof(CategoryNews) });

            if (detailNews == null)
            {
                throw new BusinessException(ErrorCodes.DetailNewsNotFound);
            }

            var detailNewDto = new DetailNewsDto(detailNews);

            var viewNews = detailNews.ViewNews;
            viewNews.Count++;

            viewNewsRepository.Update(viewNews);
            await unitOfWork.SaveChangesAsync();

            return detailNewDto;
        }

        public async Task UpdateAsync(Guid id, DetailNewsRequest request)
        {
            var detailNews = await detailNewsRepository.GetByIdAsync(id, new string[] { nameof(ViewNews) });

            if (detailNews == null)
            {
                throw new BusinessException(ErrorCodes.DetailNewsNotFound);
            }

            var oldStatus = detailNews.Status;
            if (detailNews.CategoryNews.Id != request.CategoryNewsId)
            {
                var category = await categoryNewsRepository.GetByIdAsync(request.CategoryNewsId);
                detailNews.CategoryNews = category;
            }
            detailNews.Title = request.Title;
            detailNews.Content = request.Content;
            detailNews.ExpiredDate = request.ExpiredDate;
            detailNews.Status = request.Status;

            detailNewsRepository.Update(detailNews);
            var isSuccess = await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var detailNews = await detailNewsRepository.GetByIdAsync(id);
            if (detailNews == null)
            {
                throw new BusinessException(ErrorCodes.DetailNewsNotFound);
            }

            detailNewsRepository.Remove(detailNews);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
