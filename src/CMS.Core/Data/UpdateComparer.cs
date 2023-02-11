using System;
using System.Linq.Expressions;

namespace CMS.Core.Data
{
    public class UpdateComparer<T>
    {
        public Expression<Func<T, object>> Prop { get; set; }
        public Func<T, T, bool> DiffComparer { get; set; }

        public UpdateComparer(Expression<Func<T, object>> prop, Func<T, T, bool> diffComparer = null)
        {
            Prop = prop;
            DiffComparer = diffComparer;
        }
    }
}
