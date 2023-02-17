using CMS.Core.Data.Entities;
using CMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CMS.Infrastructure.Extensions
{
    public static class DbSetExtension
    {
        public static void AttachIfNeed<T>(this DbSet<T> table, T entity, ApplicationDbContext context) where T : BaseEntity
        {
            var state = context.Entry(entity).State;
            if (state == EntityState.Detached)
            {
                table.Attach(entity);
            }
        }
    }
}
