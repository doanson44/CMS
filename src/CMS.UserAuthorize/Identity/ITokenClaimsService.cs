namespace CMS.IdentityUserLib.Identity;

public interface ITokenClaimsService
{
    Task<string> GetTokenAsync(string userName);
}
