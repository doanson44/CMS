using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CMS.Core.Helpers;

public static class DataValidationHelper
{
    /// <summary>
    /// Check if given email address is valid or not
    /// </summary>
    /// <param name="email">the email address</param>
    /// <returns>True if valid</returns>
    public static bool IsValidEmail(string email)
    {
        return !string.IsNullOrWhiteSpace(email) && new EmailAddressAttribute().IsValid(email);
    }

    public static bool IsValidPassword(string password)
    {
        var pattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$";
        var rg = new Regex(pattern);
        return rg.IsMatch(password);
    }
}
