using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CMS.Core.Domains.Shared;
using Microsoft.EntityFrameworkCore;

namespace CMS.Core.Data.Extensions;

public static class IQueryableExtension
{
    public static async Task<PagedList<T>> ToPagedListAsync<T>(
        this IQueryable<T> source,
        int page,
        int take,
        CancellationToken cancellationToken = default)
    {
        page = Math.Max(page, 1);
        var skip = (page - 1) * take;
        take = take < 0 ? int.MaxValue : take;

        var total = await source.CountAsync(cancellationToken);
        var items = await source.Skip(skip).Take(take).ToListAsync(cancellationToken);

        return new PagedList<T>(total, items);
    }

    public static async Task<List<T>> ToPagedListWithoutTotalAsync<T>(
        this IQueryable<T> source,
        int page,
        int take,
        CancellationToken cancellationToken = default)
    {
        page = Math.Max(page, 1);
        var skip = (page - 1) * take;
        take = take < 0 ? int.MaxValue : take;

        var items = await source.Skip(skip).Take(take).ToListAsync(cancellationToken);

        return new List<T>(items);
    }

    public static IOrderedQueryable<T> Sort<T>(this IQueryable<T> query, Expression<Func<T, object>> orderBy, bool asc = true)
    {
        if (asc)
        {
            return query.OrderBy(orderBy);
        }
        else
        {
            return query.OrderByDescending(orderBy);
        }
    }
}
