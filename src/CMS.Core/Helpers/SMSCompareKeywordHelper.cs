using System;
using System.Collections.Generic;
using System.Linq;

namespace CMS.Core.Helpers;

public static class SMSCompareKeywordHelper
{
    public static bool MatchedKeyword(this string keywords, IList<string> keywordMatches)
    {
        if (keywordMatches.Count == 0 || string.IsNullOrEmpty(keywords))
        {
            return false;
        }

        return keywordMatches.Any(key => keywords.Trim().Equals(key, StringComparison.OrdinalIgnoreCase));
    }

    public static bool ContainsKeyword(this string keywords, IList<string> keywordMatches)
    {
        if (keywordMatches.Count == 0 || string.IsNullOrEmpty(keywords))
        {
            return false;
        }

        return keywordMatches.Any(key => keywords.Trim().ToLower().Contains(key.ToLower()));
    }
}
