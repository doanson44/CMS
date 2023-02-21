using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CMS.Core.Extensions;

public static class CollectionExtension
{
    public static bool HasChanged(this IDictionary<string, object> left, IDictionary<string, object> right)
    {
        if (left.Count != right.Count)
            throw new Exception("Data must has same keys");

        foreach (var key in left.Keys)
        {
            if (!right.ContainsKey(key))
                throw new Exception($"Key {key} not found in comparer data");

            var leftValue = left[key];
            var rightValue = right[key];

            if (!Equals(leftValue, rightValue))
            {
                Debug.WriteLine($"Key {key} has changed from [{rightValue}] -> [{leftValue}]");
                return true;
            }
        }

        return false;
    }

    public static void AddIfPresent(this List<string> list, string newItem)
    {
        if (!string.IsNullOrWhiteSpace(newItem))
        {
            list.Add(newItem);
        }
    }

    public static void AddRangeIfPresent(this List<string> list, IEnumerable<string> newItems)
    {
        if (newItems?.Any() != true)
        {
            return;
        }

        foreach (var item in newItems)
        {
            list.AddIfPresent(item);
        }
    }

    public static List<T> UniqBy<T, TKey>(this List<T> list, Func<T, TKey> groupFunc)
    {
        if (list == null || list.Count == 0)
        {
            return new List<T>();
        }

        return list.GroupBy(groupFunc).Select(x => x.First()).ToList();
    }

    /// <summary>
    /// Format fullname to First name Titlecase and LASTNAME to uppercase
    /// </summary>
    /// <param name="firstName">The first name.</param>
    /// <param name="lastName">The last name.</param>
    /// <returns></returns>
    public static string FormatFullName(this string firstName, string lastName = null)
    {
        var result = string.Empty;
        if (!string.IsNullOrWhiteSpace(firstName))
        {
            result = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(firstName.ToLower());
        }
        if (!string.IsNullOrWhiteSpace(lastName))
        {
            result += string.IsNullOrWhiteSpace(result) ? lastName.ToUpper() : " " + lastName.ToUpper();
        }
        return result;
    }

    /// <summary>
    /// Format lastname to Last Name Titlecase
    /// </summary>
    /// <param name="lastName">The last name.</param>
    /// <returns></returns>
    public static string FormatTitlecase(this string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
        {
            return string.Empty;
        }
        lastName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(lastName.ToLower());
        return lastName;
    }
}
