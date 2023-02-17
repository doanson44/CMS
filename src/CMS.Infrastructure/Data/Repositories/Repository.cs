using CMS.Core.Data;
using CMS.Core.Data.Entities;
using CMS.Core.Data.Extensions;
using CMS.Core.Data.Repositories;
using CMS.Core.Domains.Shared;
using CMS.Core.Enums;
using CMS.Core.Services.Interfaces;
using CMS.Core.Settings;
using CMS.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace CMS.Infrastructure.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
        protected readonly ApplicationDbContext Context;
        private readonly DbSet<T> table;
        public IQueryable<T> All { get; set; }
        public IQueryable<T> AllWithDeleted { get; set; }
        private readonly IAuthenticationServices authenticationServices;
        private readonly ProductionTestingSetting productionTesting;
        private readonly string tableName;

        public Repository(ApplicationDbContext context, IAuthenticationServices authenticationServices, IOptions<ProductionTestingSetting> productionTesting)
        {
            Context = context;

            table = context.Set<T>();

#if DEBUG
            tableName = typeof(T).Name;
#endif

            this.authenticationServices = authenticationServices;
            this.productionTesting = productionTesting.Value;

            SetupSource();
        }

        private void SetupSource()
        {
            var source = table.AsNoTracking();

            // testing enabled and user logged in
            if (productionTesting.Enable && authenticationServices.IsAuthenticated)
            {
                var (id, name) = authenticationServices.GetCurrentUser();
                var testingUsers = productionTesting.GetUsers();
                if (productionTesting.IsTestingUser(id.ToString()))
                {
                    // only view the data created or belong to testing account
                    source = source.Where(x => testingUsers.Any(u => u == x.CreatedBy));
                }
                else
                {
                    // filter records which is not created by these testing users
                    source = source.Where(x => string.IsNullOrEmpty(x.CreatedBy) || !testingUsers.Any(u => u == x.CreatedBy));
                }
            }

            All = source;
            AllWithDeleted = source.IgnoreQueryFilters();
        }

        private string GetCurrentUserId()
        {
            var currentUserId = authenticationServices.GetCurrentUser().id;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return string.Empty;
            }

            return currentUserId;
        }

        private string GetCurrentUserName()
        {
            var currentUserId = authenticationServices.GetCurrentUser().userName;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return string.Empty;
            }

            return currentUserId;
        }

        public void Add(T entity)
        {
            var now = DateTime.Now;
            var currentUserName = GetCurrentUserName();
            if (entity.CreatedAt == DateTime.MinValue)
            {
                entity.CreatedAt = now;
                entity.CreatedBy = currentUserName;
            }
            entity.UpdatedAt = now;
            entity.UpdatedBy = currentUserName;

            table.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            var now = DateTime.Now;
            var currentUserName = GetCurrentUserName();
            foreach (var entity in entities)
            {
                if (entity.CreatedAt == DateTime.MinValue)
                {
                    entity.CreatedAt = now;
                    entity.CreatedBy = currentUserName;
                }

                entity.UpdatedAt = now;
                entity.UpdatedBy = currentUserName;

                table.Add(entity);
            }
        }

        public async Task RemoveAsync(T entity, bool really = false)
        {
            var now = DateTime.Now;
            var currentUserName = GetCurrentUserName();

            if (really)
            {
                table.Remove(entity);
            }
            else
            {
                entity.DeletedAt = now;
                entity.UpdatedBy = currentUserName;
                await UpdateAsync(entity, new List<Expression<Func<T, object>>> { x => x.DeletedAt, u => u.UpdatedBy });
            }
        }

        public async Task<int> BatchRemove(Expression<Func<T, bool>> predicate, bool really = false)
        {
            var currentUserName = GetCurrentUserName();
            var entities = All.Where(predicate);
            int deleted;

            if (really)
            {
                deleted = await entities.DeleteAsync();
            }
            else
            {
                var currentUserGuidId = currentUserName;
                deleted = await entities.UpdateAsync(_ => new T { DeletedAt = DateTime.Now, UpdatedBy = currentUserGuidId });
            }

            Debug.WriteLine($"> {deleted} {tableName} records deleted");

            return deleted;
        }

        public async Task<int> BatchRemoveForCleanUpResourcesMadebyTestingAccountOnly(Expression<Func<T, bool>> predicate, bool really = false)
        {
            var currentUserName = GetCurrentUserName();
            var entities = table.AsNoTracking().Where(predicate); // use table directly
            int deleted;

            if (really)
            {
                deleted = await entities.DeleteAsync();
            }
            else
            {
                var currentUserGuidId = currentUserName;
                deleted = await entities.UpdateAsync(_ => new T { DeletedAt = DateTime.Now, UpdatedBy = currentUserGuidId });
            }

            Debug.WriteLine($"> {deleted} {tableName} records deleted");

            return deleted;
        }

        public virtual Task<T> UpdateAsync(T updating, List<Expression<Func<T, object>>> updateProperties = null)
        {
            updating.UpdatedAt = DateTime.Now;
            var currentUserName = GetCurrentUserName();

            if (!string.IsNullOrEmpty(currentUserName))
            {
                updating.UpdatedBy = currentUserName;
            }

            if (updateProperties?.Count > 0)
            {
                // entity must be attached before update any properties
                table.AttachIfNeed<T>(updating, Context);

                foreach (var p in updateProperties)
                {
                    Context.Entry(updating).Property(p).IsModified = true;
                }

                Context.Entry(updating).Property(x => x.UpdatedAt).IsModified = true;
            }
            else
            {
                Context.Entry(updating).State = EntityState.Modified; // update all properties
            }

            if (updating.CreatedAt == DateTime.MinValue)
                Context.Entry(updating).Property(x => x.CreatedAt).IsModified = false;

            Context.Entry(updating).Property(x => x.UpdatedBy).IsModified = true;

            return Task.FromResult(updating);
        }

        public async Task<int> BatchUpdate(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> factory)
        {
            var currentUserName = GetCurrentUserName();
            var saved = await AllWithDeleted.Where(predicate).UpdateAsync(_ => new T { UpdatedAt = DateTime.Now, UpdatedBy = currentUserName });
            saved = await AllWithDeleted.Where(predicate).UpdateAsync(factory);

            Debug.WriteLine($"> Total {saved} {tableName} records saved");
            return saved;
        }

        public Task<int> RestoreAsync(Expression<Func<T, bool>> predicate)
        {
            return AllWithDeleted.Where(predicate).UpdateAsync(_ => new T { DeletedAt = null });
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
            var model = await table.FindAsync(id);

            if (includeProperties == null || includeProperties.Length == 0)
            {
                return model;
            }

            foreach(var item in includeProperties)
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

        public Task<T> GetOneDeletedAsync(Expression<Func<T, bool>> predicate, string[] includeProperties = null)
        {
            return GetDeleted(includeProperties).Where(predicate).OrderByDescending(x => x.UpdatedAt).FirstOrDefaultAsync();
        }

        public Task<TResult> GetOneDeletedAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, string[] includeProperties = null)
        {
            return GetDeleted(includeProperties).Where(predicate).OrderByDescending(x => x.UpdatedAt).Select(selector).FirstOrDefaultAsync();
        }

        public Task<T> GetOneAsync(ISpecification<T> spec, string[] includeProperties = null)
        {
            return ApplySpecification(spec, includeProperties).SingleOrDefaultAsync();
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
            var found = await items.Where(x => x.DeletedAt != null).AnyAsync(where);

            if (found)
            {
                return Core.Enums.EntityExistanceState.Archived;
            }

            found = await items.Where(x => x.DeletedAt == null).AnyAsync(where);
            return found ? Core.Enums.EntityExistanceState.Live : Core.Enums.EntityExistanceState.None;
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
            var items = table;

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
