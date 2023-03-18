using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Core.Constants;
using CMS.Core.Data.Entites;
using CMS.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CMS.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = RoleConstants.PermisstionType.Administrators)]
public class UserRolesController : BaseApiController
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserRolesController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync(string username)
    {
        var viewModel = new List<UserRolesViewModel>();
        var user = await _userManager.FindByNameAsync(username);
        foreach (var role in _roleManager.Roles.ToList())
        {
            var userRolesViewModel = new UserRolesViewModel
            {
                RoleName = role.Name
            };
            if (await _userManager.IsInRoleAsync(user, role.Name))
            {
                userRolesViewModel.Selected = true;
            }
            else
            {
                userRolesViewModel.Selected = false;
            }
            viewModel.Add(userRolesViewModel);
        }
        var model = new ManageUserRolesViewModel()
        {
            Username = username,
            UserRoles = viewModel
        };

        return Ok(model);
    }

    [HttpPut("{username}")]
    public async Task<IActionResult> Update([FromRoute] string username, ManageUserRolesViewModel model)
    {
        var user = await _userManager.FindByNameAsync(username);
        var roles = await _userManager.GetRolesAsync(user);
        var result = await _userManager.RemoveFromRolesAsync(user, roles);
        result = await _userManager.AddToRolesAsync(user, model.UserRoles.Where(x => x.Selected).Select(y => y.RoleName));
        var currentUser = await _userManager.GetUserAsync(User);
        await _signInManager.RefreshSignInAsync(currentUser);
        return Ok();
    }

    [HttpPost("copy-roles")]
    public async Task<IActionResult> CopyRoles(CopyRolesViewModel model)
    {
        var fromUser = await _userManager.FindByNameAsync(model.CopyFromUser);
        var fromUserRoles = await _userManager.GetRolesAsync(fromUser);

        var toUser = await _userManager.FindByNameAsync(model.CopyToUser);
        var toUserRoles = await _userManager.GetRolesAsync(toUser);

        var result = await _userManager.RemoveFromRolesAsync(toUser, toUserRoles);
        result = await _userManager.AddToRolesAsync(toUser, fromUserRoles);
        var currentUser = await _userManager.GetUserAsync(User);
        await _signInManager.RefreshSignInAsync(currentUser);
        return Ok();
    }
}
