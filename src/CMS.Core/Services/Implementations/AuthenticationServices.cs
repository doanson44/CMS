using System.Security.Claims;
using CMS.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CMS.Core.Services;

public class AuthenticationServices : IAuthenticationServices
{
    private readonly IHttpContextAccessor _contextAccessor;
    private string id = string.Empty;
    private string userName = string.Empty;

    public AuthenticationServices(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public bool IsAuthenticated => _contextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public (string id, string userName) GetCurrentUser()
    {
        if (!IsAuthenticated)
        {
            return (string.Empty, string.Empty);
        }

        if (string.IsNullOrWhiteSpace(id))
        {
            var user = _contextAccessor.HttpContext.User;
            if (user != null)
            {
                id = user.FindFirst(ClaimTypes.Name).Value;
                userName = user.FindFirst(ClaimTypes.Name).Value;
            }
        }

        return (id, userName);
    }
}
