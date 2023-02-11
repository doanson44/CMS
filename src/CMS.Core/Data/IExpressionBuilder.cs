using System;
using System.Linq.Expressions;

namespace CMS.Core.Data
{
    public interface IExpressionBuilder<T>
    {
        Expression<Func<T, bool>> ToExpression();
    }

    public class TrueExpression<T> : IExpressionBuilder<T>
    {
        public Expression<Func<T, bool>> ToExpression()
        {
            return _ => true;
        }
    }

    public class FalseExpression<T> : IExpressionBuilder<T>
    {
        public Expression<Func<T, bool>> ToExpression()
        {
            return _ => false;
        }
    }
}
