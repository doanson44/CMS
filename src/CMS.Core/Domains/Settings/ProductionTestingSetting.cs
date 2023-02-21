using System.Linq;

namespace CMS.Core.Settings;

public class ProductionTestingSetting
{
    public const string ConfigKey = "ProductionTesting";
    public bool Enable { get; set; }

    /// <summary>
    /// user id seperate by comma
    /// </summary>
    public string Users { get; set; }

    /// <summary>
    /// setup forward emails, using command to add multiple email (email1,email2,email3)
    /// </summary>
    /// <value></value>
    public string EmailForward { get; set; }
    public string SmsForward { get; set; }

    /// <summary>
    /// Key used for admin registration
    /// </summary>
    public string RegisterKey { get; set; }

    public string[] GetUsers()
    {
        if (string.IsNullOrWhiteSpace(Users))
        {
            return System.Array.Empty<string>();
        }

        return Users.Split(',').Select(x => x.Trim()).ToArray();
    }

    public bool IsTestingUser(string userId)
    {
        // not allow anonymous user
        if (string.IsNullOrWhiteSpace(userId))
        {
            return false;
        }

        var configuredUsers = GetUsers();
        return configuredUsers.Length > 0 && configuredUsers.Any(u => u == userId);
    }

    public bool IsValidSecretKey(string key)
    {
        return !string.IsNullOrWhiteSpace(key) && key == RegisterKey;
    }
}
