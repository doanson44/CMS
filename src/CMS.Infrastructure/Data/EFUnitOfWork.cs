using CMS.Core.Data;
using CMS.Core.Data.Repositories;
using CMS.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Infrastructure.Data
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext context;
        private readonly IServiceProvider servicesProvider;
        private bool disposed;
        private readonly IAuthenticationServices authenticationServices;

        public EFUnitOfWork(ApplicationDbContext context, IServiceProvider servicesProvider, IAuthenticationServices authenticationServices)
        {
            this.context = context;
            this.servicesProvider = servicesProvider;
            this.authenticationServices = authenticationServices;
        }

        public async Task<int> SaveChangesAsync()
        {
            var saved = 0;
            try
            {
                saved = await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                try
                {
                    if (ex?.InnerException != null)
                    {
                        var entriesEx = context.ChangeTracker.Entries().Where((x => x.State == EntityState.Added || x.State == EntityState.Modified));
                        foreach (var entry in entriesEx)
                        {
                            foreach (var prop in entry.CurrentValues.Properties)
                            {
                                Console.WriteLine("End======================================================================");
                                var val = prop.PropertyInfo.GetValue(entry.Entity);
                                Console.WriteLine($"Handle Error: {prop?.ToString()} ~ ({val?.ToString().Length})({val})");
                            }
                        }
                    }
                }
                catch (Exception except)
                {
                    //throw;
                }
            }

            Debug.WriteLine($"> {saved} records saved");

            return saved;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposed)
        {
            if (this.disposed)
                return;

            this.disposed = disposed;
            context.Dispose();
            context = null;
        }

        public IRepository<T> Get<T>() where T : class, new()
        {
            return servicesProvider.GetRequiredService<IRepository<T>>();
        }

        [Conditional("DEBUG")]
        private static void LogEntry(EntityState state, EntityEntry log)
        {
            //Debug.WriteLine($"> {state} > {JsonConvert.SerializeObject(log.Entity, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore })}");
        }
    }
}
