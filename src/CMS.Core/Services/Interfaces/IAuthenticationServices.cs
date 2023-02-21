namespace CMS.Core.Services.Interfaces;

public interface IAuthenticationServices
{
    /// <summary>
    /// Return true if user is authenticated
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Get current user
    /// </summary>
    /// <returns>The user id and user email</returns>
    (string id, string userName) GetCurrentUser();
}
