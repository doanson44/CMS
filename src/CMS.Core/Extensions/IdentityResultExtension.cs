using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace CMS.Core.Extensions;

public static class IdentityResultExtension
{
    public static string ToError(this IdentityResult identityResult)
    {
        if (identityResult.Succeeded)
        {
            return "Success";
        }

        return identityResult.Errors.Select(err => err.Code + ": " + err.Description).JoinLines();
    }
}
