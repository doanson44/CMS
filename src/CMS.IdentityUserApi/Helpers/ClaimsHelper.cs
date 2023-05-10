using System.Reflection;
using System.Security.Claims;
using CMS.IdentityUserApi.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CMS.IdentityUserApi.Helpers;

public static class ClaimsHelper
{
    public static void GetPermissions(this List<RoleClaimsViewModel> allPermissions, Type policy, string roleName)
    {
        var fields = policy.GetFields(BindingFlags.Static | BindingFlags.Public);

        foreach (var fi in fields)
        {
            allPermissions.Add(new RoleClaimsViewModel { Value = fi.GetValue(null).ToString(), Type = "Permissions" });
        }
    }

    public static void GetPermissions(this List<RoleClaimsViewModel> allPermissions, List<string> permissions, string roleName)
    {
        foreach (var fi in permissions)
        {
            allPermissions.Add(new RoleClaimsViewModel { Value = fi, Type = "Permissions" });
        }
    }

    public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string permission)
    {
        var allClaims = await roleManager.GetClaimsAsync(role);
        if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
        {
            await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
        }
    }
}
