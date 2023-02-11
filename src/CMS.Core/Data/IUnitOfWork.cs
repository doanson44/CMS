using CMS.Core.Data.Entities;
using CMS.Core.Data.Repositories;
using System;
using System.Threading.Tasks;

namespace CMS.Core.Data
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Get the repository
        /// </summary>
        /// <typeparam name="T">entity type</typeparam>
        /// <typeparam name="TKey">entity id type</typeparam>
        /// <returns>Repository of type</returns>
        IRepository<T, TKey> Get<T, TKey>() where T : BaseEntity<TKey>, new();
    }
}
