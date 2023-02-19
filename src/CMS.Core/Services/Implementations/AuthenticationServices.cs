using CMS.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CMS.Core.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly IHttpContextAccessor contextAccessor;
        private string id = string.Empty;
        private string userName = string.Empty;

        public AuthenticationServices(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public bool IsAuthenticated => contextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public (string id, string userName) GetCurrentUser()
        {
            if (!IsAuthenticated)
            {
                return (string.Empty, string.Empty);
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                var user = contextAccessor.HttpContext.User;
                if (user != null)
                {
                    id = user.FindFirst(ClaimTypes.Name).Value;
                    userName = user.FindFirst(ClaimTypes.Name).Value;
                }
            }

            return (id, userName);
        }
    }
}
