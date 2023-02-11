using CMS.Core.Data.Entities;
using CMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CMS.Infrastructure.Extensions
{
    public static class DbSetExtension
    {
        public static void AttachIfNeed<T, TKey>(this DbSet<T> table, T entity, ApplicationDbContext context) where T : BaseEntity<TKey>
        {
            if (table.Local.Any(l => Equals(l.Id, entity.Id)))
            {
                // a trick to detach all entities currently in Local
                //table.Local.ToList().ForEach(p => context.Entry(p).State = EntityState.Detached);
                var local = table.Local.Single(x => Equals(x.Id, entity.Id));
                context.Entry(local).State = EntityState.Detached;
            }

            var state = context.Entry(entity).State;
            if (state == EntityState.Detached)
            {
                table.Attach(entity);
            }
        }
    }
}
