namespace CMS.Core.Settings;

public class JwtTokenSetting
{
    public string SecretKey { get; set; }

    public string Issuer { get; set; }

    public int ExpiryMinutes { get; set; }
}
