using System;
using System.Threading.Tasks;
using CMS.Core.Data.Repositories;

namespace CMS.Core.Data;

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
    /// <returns>Repository of type</returns>
    IRepository<T> Get<T>() where T : class, new();
}
