using CMS.Core.Domains.Shared;
using CMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CMS.Core.Data.Repositories
{
    public interface IRepository<T> where T : class, new()
    {
        /// <summary>
        /// Get all entity with condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate, string[] includeProperties = null);

        /// <summary>
        /// Get all entity with condition then select some specific values
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<List<TResult>> GetAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, string[] includeProperties = null);

        /// <summary>
        /// Get all entity include deleted 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<List<T>> GetDeletedAsync(Expression<Func<T, bool>> predicate, string[] includeProperties = null);

        /// <summary>
        /// Get paged list
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <param name="orderBy"></param>
        /// <param name="page"></param>
        /// <param name="take"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<PagedList<TResult>> GetPagedListAsync<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int page = 1,
            int take = 10,
            string[] includeProperties = null);

        /// <summary>
        /// Get paged list
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="spec"></param>
        /// <param name="selector"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<PagedList<TResult>> GetPagedListAsync<TResult>(
            ISpecification<T> spec,
            Expression<Func<T, TResult>> selector,
            string[] includeProperties = null);

        /// <summary>
        /// Get all entity with specification
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<List<T>> GetAsync(ISpecification<T> spec, string[] includeProperties = null);

        /// <summary>
        /// Get all entity with specification then select some specific values
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="selector"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<List<TResult>> GetAsync<TResult>(ISpecification<T> spec, Expression<Func<T, TResult>> selector, string[] includeProperties = null);

        /// <summary>
        /// Get by id async
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(object id, string[] includeProperties = null);
        
        /// <summary>
        /// Get single async
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<T> GetOneAsync(Expression<Func<T, bool>> predicate, string[] includeProperties = null);
        Task<TResult> GetOneAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, string[] includeProperties = null);
        Task<T> GetOneDeletedAsync(Expression<Func<T, bool>> predicate, string[] includeProperties = null);
        Task<TResult> GetOneDeletedAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, string[] includeProperties = null);
        Task<T> GetOneAsync(ISpecification<T> spec, string[] includeProperties = null);

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="really">true to soft delete</param>
        Task RemoveAsync(T entity, bool really = false);
        Task<int> BatchRemove(Expression<Func<T, bool>> predicate, bool really = false);
        Task<int> BatchRemoveForCleanUpResourcesMadebyTestingAccountOnly(Expression<Func<T, bool>> predicate, bool really = false);

        /// <summary>
        /// Restore entities
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<int> RestoreAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// update the entity
        /// </summary>
        /// <param name="updating">entity to update</param>
        /// <param name="updateProperties">update properties</param>
        /// <returns></returns>
        Task<T> UpdateAsync(T updating, List<Expression<Func<T, object>>> updateProperties = null);

        Task<int> BatchUpdate(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> factory);

        Task<EntityExistanceState> GetStateAsync(Expression<Func<T, bool>> where);
        Task<bool> IsLiveAsync(Expression<Func<T, bool>> where);
        Task<bool> IsExistAsync(Expression<Func<T, bool>> where);

        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);

        Task<TTResult> GetLatestAsync<TTResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TTResult>> selector,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            string[] includeProperties = null);

        Task<List<TResult>> GetPagedListWithoutTotalAsync<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int page = 1,
            int take = 10,
            string[] includeProperties = null);
    }
}
