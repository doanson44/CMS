using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CMS.Core.Data;

public abstract class BaseSpecification<T> : ISpecification<T>
{
    protected BaseSpecification()
    { }

    protected BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    public Expression<Func<T, bool>> Criteria { get; private set; }
    public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
    public List<string> IncludeStrings { get; } = new List<string>();
    public Expression<Func<T, object>> OrderBy { get; private set; }
    public Expression<Func<T, object>> OrderByDescending { get; private set; }
    public Expression<Func<T, object>> GroupBy { get; private set; }

    public int Take { get; private set; }
    public int Skip { get; private set; }
    public int Page { get; private set; }
    public bool IsPagingEnabled { get; set; }

    protected virtual void ApplyQuery(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected virtual void AddIncludes(params Expression<Func<T, object>>[] includes)
    {
        Includes.AddRange(includes.ToArray());
    }

    protected virtual void AddInclude(string[] includeString)
    {
        if (includeString?.Length > 0)
        {
            IncludeStrings.AddRange(includeString);
        }
    }

    protected virtual void ApplyPaging(int current, int take = 10)
    {
        if (current < 1)
            current = 1;

        Page = current;
        Skip = (current - 1) * take;
        Take = take < 0 ? int.MaxValue : take;
        IsPagingEnabled = true;
    }

    protected virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression, bool asc)
    {
        if (asc)
        {
            OrderBy = orderByExpression;
            OrderByDescending = null;
        }
        else
        {
            OrderBy = null;
            OrderByDescending = orderByExpression;
        }
    }

    protected virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        ApplyOrderBy(orderByExpression, true);
    }

    protected virtual void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        ApplyOrderBy(orderByDescendingExpression, false);
    }

    protected virtual void ApplyGroupBy(Expression<Func<T, object>> groupByExpression)
    {
        GroupBy = groupByExpression;
    }
}
