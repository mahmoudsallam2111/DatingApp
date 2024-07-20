using DatingApp.Domain.Aggregates.AppUser.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.WebApi.Controllers;

public class AdminController: BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public AdminController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    [Authorize(Policy = "RequiredAdminRole")]
    [HttpGet("GetUsersWithRole")]
    public  ActionResult GetUsersWithRoles()
    {
        var usersWithRoles = _userManager.Users
        .OrderBy(u => u.UserName)
        .Select(user => new
        {
            Id = user.Id,
            UserName = user.UserName,
            Roles = _userManager.GetRolesAsync(user).Result
        }).ToList();

        return Ok(usersWithRoles);
    }

    [Authorize(Policy = "RequiredAdminRole")]
    [HttpPut("edit-role/{userName}")]
    public async Task<ActionResult> EditRole(string userName , [FromQuery] string roles)
    {
        if (string.IsNullOrEmpty(roles))
            return BadRequest("you must select a least one role");

        var selectedRoles = roles.Split(',').ToList();

        var user = await _userManager.FindByNameAsync(userName);
        if (user == null) return NotFound();

        var userRoles = await _userManager.GetRolesAsync(user);

        var result = await _userManager.AddToRolesAsync(user , selectedRoles.Except(userRoles));
        if(!result.Succeeded) { return BadRequest("failed to add roles"); }

         result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
        if (!result.Succeeded) { return BadRequest("failed to remove from roles"); }

        return Ok(await _userManager.GetRolesAsync(user));  

    }

    [Authorize(Policy = "ModeratorOrAdminRole")]
    [HttpGet("GetphotosForModeration")]
    public ActionResult GetphotosForModeration()
    {
        return Ok();
    }
}
