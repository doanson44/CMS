using System;
using System.Collections.Generic;
using System.Linq;

namespace CMS.Core.Extensions;

public static class DictionaryExtension
{
    public enum MergeStrategy
    {
        KeepOrigin,
        Override,
    }

    /// <summary>
    /// Merge another dictionary into the first one
    /// </summary>
    /// <param name="origin">The original dictionary/param>
    /// <param name="appending">The appending dictionary</param>
    /// <param name="strategy">The merge strategy</param>
    /// <returns>The result dictionary</returns>
    public static IDictionary<string, object> Merge(this IDictionary<string, object> origin, IDictionary<string, object> appending, MergeStrategy strategy = MergeStrategy.KeepOrigin)
    {
        foreach (var key in appending.Keys)
        {
            if (!origin.ContainsKey(key)) // if not in origin dictionary, then add new key
            {
                origin.Add(key, appending[key]);
            }
            else
            {
                // override existing key
                if (strategy == MergeStrategy.Override)
                {
                    origin[key] = appending[key];
                }
            }
        }

        return origin;
    }

    /// <summary>
    /// Merge another dictionary into the first one
    /// </summary>
    /// <param name="origin">The original dictionary/param>
    /// <param name="appending">The appending dictionary</param>
    /// <param name="strategy">The merge strategy</param>
    /// <returns>The result dictionary</returns>
    public static IDictionary<string, string> Merge(this IDictionary<string, string> origin, IDictionary<string, string> appending, MergeStrategy strategy = MergeStrategy.KeepOrigin)
    {
        foreach (var key in appending.Keys)
        {
            if (!origin.ContainsKey(key)) // if not in origin dictionary, then add new key
            {
                origin.Add(key, appending[key]);
            }
            else
            {
                // override existing key
                if (strategy == MergeStrategy.Override)
                {
                    origin[key] = appending[key];
                }
            }
        }

        return origin;
    }

    /// <summary>
    /// Convert dictionary keys into lower key
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IDictionary<string, string> ToLowerKeys(this IDictionary<string, string> source)
    {
        var result = new Dictionary<string, string>();
        foreach (var key in source.Keys)
        {
            result[key.ToLower()] = source[key];
        }

        return result;
    }

    public static string GetValueByKeys(string[] keys, IDictionary<string, string> dictionary)
    {
        if (dictionary == null)
        {
            return string.Empty;
        }

        foreach (var key in keys)
        {
            var (keyExists, originalKey) = dictionary.ContainsKeyIgnoreCase(key);
            if (keyExists)
            {
                return dictionary[originalKey ?? key];
            }
        }

        return string.Empty;
    }

    private static (bool, string) ContainsKeyIgnoreCase<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
        bool? keyExists;
        var originalKey = string.Empty;
        if (key is string keyString)
        {
            originalKey =
                dictionary.Keys.OfType<string>()
                .FirstOrDefault(k => string.Equals(k, keyString, StringComparison.InvariantCultureIgnoreCase));

            keyExists = !string.IsNullOrEmpty(originalKey);
        }
        else
        {
            // Key is any other type, use default comparison.
            keyExists = dictionary.ContainsKey(key);
        }

        return (keyExists ?? false, originalKey);
    }
}
