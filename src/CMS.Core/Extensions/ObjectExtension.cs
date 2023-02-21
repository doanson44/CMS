using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using CMS.Core.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMS.Core.Extensions;

public static class ObjectExtension
{
    public static string GetPropertyName<T>(this Expression<Func<T, object>> property)
    {
        var lambda = (LambdaExpression)property;
        MemberExpression memberExpression;

        if (lambda.Body is UnaryExpression unaryExpression)
        {
            memberExpression = (MemberExpression)unaryExpression.Operand;
        }
        else
        {
            memberExpression = (MemberExpression)lambda.Body;
        }

        return memberExpression.Member.Name;
    }

    /// <summary>
    /// Convert object into dictionary
    /// </summary>
    /// <param name="source"></param>
    /// <param name="baseName"></param>
    /// <returns></returns>
    public static IDictionary<string, object> AsDictionary(this object source, string baseName = null)
    {
        if (source == null)
        {
            return null;
        }

        var bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;
        var props = source.GetType().GetProperties(bindingAttr);
        var dictionary = new Dictionary<string, object>();

        foreach (var p in props)
        {
            var name = string.IsNullOrWhiteSpace(baseName) ? p.Name : baseName + "." + p.Name;
            Debug.WriteLine(name);
            var val = p.GetValue(source, null);

            if (val == null)
            {
                dictionary[name] = string.Empty;
                continue;
            }

            var type = val.GetType();

            if (type.IsPrimitive || type == typeof(string))
            {
                dictionary[name] = val;
            }
            else if (type == typeof(DateTime))
            {
                dictionary[name] = ((DateTime)val).ToString("dd/MM/yyyy");
            }
            else if (type.IsGenericType) // generic
            {
                var typeDef = type.GetGenericTypeDefinition();
                if (typeDef == typeof(List<>) || typeDef == typeof(IEnumerable<>) || typeDef == typeof(ICollection<>) || typeDef == typeof(IList<>))
                {
                    // TODO support list to dictionary
                }
                else
                {
                    dictionary.Merge(val.AsDictionary(name));
                }
            }
            else // object
            {
                dictionary.Merge(val.AsDictionary(name));
            }
        }

        return dictionary;
    }

    /// <summary>
    /// Convert the source dictionary into flat dictionary
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IDictionary<string, string> Flatten(this IDictionary<string, object> source)
    {
        if (source == null || source.Keys.Count == 0)
        {
            return null;
        }

        var dictionary = new Dictionary<string, string>();
        foreach (var name in source.Keys)
        {
            var val = source[name];

            if (val == null)
            {
                dictionary[name] = string.Empty;
                continue;
            }

            var type = val.GetType();

            if (type.IsPrimitive || type == typeof(string))
            {
                dictionary[name] = val.ToString();
            }
            else if (type == typeof(DateTime))
            {
                if (name.ToLower().Contains("datetime"))
                {
                    dictionary[name] = ((DateTime)val).ToString("dd/MM/yyyy HH:mm");
                }
                else if (name.ToLower().Contains("time"))
                {
                    dictionary[name] = ((DateTime)val).ToString("HH:mm");
                }
                else
                {
                    dictionary[name] = ((DateTime)val).ToString("dd/MM/yyyy");
                }

            }
            else if (type.IsGenericType) // generic
            {
                var typeDef = type.GetGenericTypeDefinition();
                if (typeDef == typeof(List<>) || typeDef == typeof(IEnumerable<>) || typeDef == typeof(ICollection<>) || typeDef == typeof(IList<>))
                {
                    // TODO support list
                }
                else
                {
                    dictionary[name] = string.Empty;
                }
            }
            else if (type == typeof(JObject)) // jobject => deserialize it into dictionary and flatten again
            {
                var subDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(val.ToString());
                if (subDic.Count > 0)
                {
                    dictionary.Merge(subDic.Flatten());
                }
            }
            else // object => convert into dictionary and then flatten, normally it won't happen because the deserialization won't result an object
            {
                dictionary.Merge(val.AsDictionary().Flatten());
            }
        }

        return dictionary;
    }

    public static List<Expression<Func<T, object>>> GetChangedProperties<T>(this T updating, T entity, List<UpdateComparer<T>> compares) where T : class, new()
    {
        var updateProps = new List<Expression<Func<T, object>>>();

        foreach (var compare in compares)
        {
            if (HasChanged(updating, entity, compare))
            {
                updateProps.Add(compare.Prop);
            }
        }

        return updateProps;
    }

    private static bool HasChanged<T>(T updating, T entity, UpdateComparer<T> compare) where T : class, new()
    {
        if (compare.DiffComparer != null)
        {
            return compare.DiffComparer(updating, entity);
        }

        var p = compare.Prop;
        var origin = p.Compile().Invoke(entity);
        var updatingVal = p.Compile().Invoke(updating);

        var equal = Equals(origin, updatingVal);
#if DEBUG
        if (!equal)
        {
            Debug.WriteLine($"> property {p.GetPropertyName()} has changed {origin} -> {updatingVal}");
        }
#endif

        return !equal;
    }

}
