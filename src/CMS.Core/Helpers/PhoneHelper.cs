using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CMS.Core.Helpers;

public static class PhoneHelper
{
    public static string AddAuPhonePrefix(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return string.Empty;

        phone = Regex.Replace(phone, @"\s+", "");
        if (phone.StartsWith("+"))
        {
            return phone;
        }
        else
        {
            return $"+61{phone.TrimStart('0')}";
        }
    }

    public static string RemoveCountryCode(string phonenumber)
    {
        var countriesList = new List<string>
        {
            "+61",
            "+84"
        };

        foreach (var country in countriesList)
        {
            phonenumber = phonenumber.Replace(country, "");
        }

        return phonenumber;
    }
}

