using CMS.Core.Data;
using CMS.Core.Data.Extensions;
using CMS.Core.Data.Repositories;
using CMS.Core.Domains.Shared;
using CMS.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace CMS.Infrastructure.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        protected readonly ApplicationDbContext Context;
        private readonly DbSet<T> dbSet;
        public IQueryable<T> All { get; set; }
        public IQueryable<T> AllWithDeleted { get; set; }
        private readonly string tableName;

        public Repository(ApplicationDbContext context)
        {
            Context = context;

            dbSet = context.Set<T>();

#if DEBUG
            tableName = typeof(T).Name;
#endif
            SetupSource();
        }

        private void SetupSource()
        {
            var source = dbSet.AsNoTracking();
            All = source;
            AllWithDeleted = source.IgnoreQueryFilters();
        }

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public async Task<int> BatchRemoveAsync(Expression<Func<T, bool>> predicate)
        {
            return await All.Where(predicate).DeleteAsync(); ;
        }

        public async Task<int> BatchRemoveForCleanUpResourcesMadebyTestingAccountOnlyAsync(Expression<Func<T, bool>> predicate)
        {
            var entities = dbSet.AsNoTracking().Where(predicate); // use table directly
            return await entities.DeleteAsync(); ;
        }

        public virtual void Update(T updating, List<Expression<Func<T, object>>> updateProperties = null)
        {
            dbSet.Attach(updating);
            Context.Entry(updating).State = EntityState.Modified;
        }

        public async Task<int> BatchUpdateAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> factory)
        {
            var saved = await AllWithDeleted.Where(predicate).UpdateAsync(factory);
            return saved;
        }

        public Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate, string[] includeProperties = null)
        {
            return Get(predicate, includeProperties).ToListAsync();
        }

        public Task<List<T>> GetDeletedAsync(Expression<Func<T, bool>> predicate, string[] includeProperties = null)
        {
            return GetDeleted(predicate, includeProperties).ToListAsync();
        }

        public Task<List<TResult>> GetAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, string[] includeProperties = null)
        {
            return Get(predicate, includeProperties).Select(selector).ToListAsync();
        }

        public Task<PagedList<TResult>> GetPagedListAsync<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int page = 1,
            int take = 10,
            string[] includeProperties = null)
        {
            var source = Get(predicate, includeProperties);

            if (orderBy != null)
            {
                source = orderBy(source);
            }

            return source.Select(selector).ToPagedListAsync(page, take);
        }

        public Task<PagedList<TResult>> GetPagedListAsync<TResult>(
            ISpecification<T> spec,
            Expression<Func<T, TResult>> selector,
            string[] includeProperties = null)
        {
            spec.IsPagingEnabled = false; // if using ToPagedListAsync then skip default paging
            return ApplySpecification(spec, includeProperties).Select(selector).ToPagedListAsync(spec.Page, spec.Take);
        }

        public Task<List<T>> GetAsync(ISpecification<T> spec, string[] includeProperties = null)
        {
            return ApplySpecification(spec, includeProperties).ToListAsync();
        }

        public Task<List<TResult>> GetAsync<TResult>(ISpecification<T> spec, Expression<Func<T, TResult>> selector, string[] includeProperties = null)
        {
            return ApplySpecification(spec, includeProperties).Select(selector).ToListAsync();
        }

        public async Task<T> GetByIdAsync(object id, string[] includeProperties = null)
        {
            var model = await dbSet.FindAsync(id);

            if (includeProperties == null || includeProperties.Length == 0)
            {
                return model;
            }

            foreach (var item in includeProperties)
            {
                await Context.Entry(model).Reference(item).LoadAsync();
            }

            return model;
        }

        public Task<T> GetOneAsync(Expression<Func<T, bool>> predicate, string[] includeProperties = null)
        {
            return Get(includeProperties).FirstOrDefaultAsync(predicate);
        }

        public Task<TResult> GetOneAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, string[] includeProperties = null)
        {
            return Get(predicate, includeProperties).Select(selector).SingleOrDefaultAsync();
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return All.CountAsync();
            }

            return All.CountAsync(predicate);
        }

        public async Task<EntityExistanceState> GetStateAsync(Expression<Func<T, bool>> where)
        {
            var items = AllWithDeleted;
            var found = await items.AnyAsync(where);

            if (found)
            {
                return EntityExistanceState.Archived;
            }

            found = await items.AnyAsync(where);
            return found ? EntityExistanceState.Live : EntityExistanceState.None;
        }

        public Task<bool> IsLiveAsync(Expression<Func<T, bool>> where)
        {
            return All.AnyAsync(where);
        }

        public Task<bool> IsExistAsync(Expression<Func<T, bool>> where)
        {
            return AllWithDeleted.AnyAsync(where);
        }

        public Task<TResult> GetLatestAsync<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            string[] includeProperties = null)
        {
            var source = Get(predicate, includeProperties);
            if (orderBy != null)
            {
                source = orderBy(source);
            }

            return source.Select(selector).FirstOrDefaultAsync();
        }

        public Task<List<TResult>> GetPagedListWithoutTotalAsync<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int page = 1,
            int take = 10,
            string[] includeProperties = null)
        {
            var source = Get(predicate, includeProperties);

            if (orderBy != null)
            {
                source = orderBy(source);
            }

            return source.Select(selector).ToPagedListWithoutTotalAsync(page, take);
        }

        #region Private Methods
        private IQueryable<T> Get(string[] includeProperties = null)
        {
            var items = All;

            if (includeProperties?.Length > 0)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }

            return items;
        }

        private DbSet<T> IncludeProperties(string[] includeProperties = null)
        {
            var items = dbSet;

            if (includeProperties?.Length > 0)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items.Include(includeProperty);
                }
            }

            return items;
        }

        private IQueryable<T> GetDeleted(string[] includeProperties = null)
        {
            var items = AllWithDeleted;

            if (includeProperties?.Length > 0)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }

            return items;
        }

        private IQueryable<T> Get(Expression<Func<T, bool>> predicate, string[] includeProperties = null)
        {
            return Get(includeProperties).Where(predicate);
        }

        private IQueryable<T> GetDeleted(Expression<Func<T, bool>> predicate, string[] includeProperties = null)
        {
            return GetDeleted(includeProperties).Where(predicate);
        }

        protected IQueryable<T> ApplySpecification(ISpecification<T> spec, string[] includeProperties = null)
        {
            return SpecificationEvaluator<T>.GetQuery(Get(includeProperties), spec);
        }
        #endregion
    }
}
