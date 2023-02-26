using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Core.Constants;
using CMS.WebApi.Helpers;
using CMS.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CMS.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = RoleConstants.PermisstionType.Administrators)]
public class PermissionController : BaseApiController
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public PermissionController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync(string roleName)
    {
        var model = new PermissionViewModel();
        var allPermissions = new List<RoleClaimsViewModel>();
        allPermissions.GetPermissions(Security.AllPermissions, roleName);
        var role = await _roleManager.FindByNameAsync(roleName);
        model.RoleName = roleName;
        var claims = await _roleManager.GetClaimsAsync(role);
        var allClaimValues = allPermissions.Select(a => a.Value).ToList();
        var roleClaimValues = claims.Select(a => a.Value).ToList();
        var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
        foreach (var permission in allPermissions)
        {
            if (authorizedClaims.Any(a => a == permission.Value))
            {
                permission.Selected = true;
            }
        }
        model.RoleClaims = allPermissions;
        return Ok(model);
    }

    [HttpPost]
    public async Task<IActionResult> Update(PermissionViewModel model)
    {
        var role = await _roleManager.FindByNameAsync(model.RoleName);
        var claims = await _roleManager.GetClaimsAsync(role);
        foreach (var claim in claims)
        {
            await _roleManager.RemoveClaimAsync(role, claim);
        }
        var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
        foreach (var claim in selectedClaims)
        {
            await _roleManager.AddPermissionClaim(role, claim.Value);
        }
        return Ok();
    }
}
