using AuthModule.Data.Models.Abstract;
using AuthModule.Data.Models;
using AuthModule.Data;
using AuthModule.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthModule.Controllers.Abstract;

[ApiController]
[Route("api/auth/role")]
public class Roles<TUser, TUserId> : ControllerBase
   where TUser : class, IUser<TUser, TUserId>
{
    private readonly AuthDbContxt<TUser, TUserId> _authDbContxt;

    public Roles(AuthDbContxt<TUser, TUserId> authDbContxt)
    {
        _authDbContxt = authDbContxt;
    }

    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _authDbContxt.Roles.Select(x => new { x.Id, x.Name }).ToListAsync();
        return Ok(roles);
    }

    [HttpGet]
    public async Task<IActionResult> GetUserRoles(TUserId userId)
    {
        TUser user;
        try
        {
            user = await _authDbContxt.Users.Include(x => x.Roles).SingleAsync(x => x.Id.Equals(userId));
        }
        catch (InvalidOperationException)
        {
            return NotFound("no user with this id");
        }

        var roles = user.Roles.Select(x => new { x.Id, x.Name });
        return Ok(roles);
    }

    [HttpPost]
    public async Task<IActionResult> AddRoles(List<string> roles)
    {
        List<Role<TUser>> r = roles.Select(x => new Role<TUser>() { Name = x}).ToList();

        await _authDbContxt.Roles.AddRangeAsync(r);
        await _authDbContxt.SaveChangesAsync();

        return Ok();
    }

    [HttpPost]
    [Route("claims")]
    public async Task<IActionResult> AddClaimsToRole(List<int> claimIds, int roleId)
    {
        var role = await _authDbContxt.Roles.FindAsync(roleId);

        if (role == null)
            return NotFound("no role with this id");
        
        // shit code \\
        var claims = _authDbContxt.Claims.Where(x => claimIds.Contains(x.Id));
        role.Claims.AddRange(claims);
        // shit code \\

        await _authDbContxt.SaveChangesAsync();
        return Ok();
    }

    [HttpGet]
    [Route("claims")]
    public async Task<IActionResult> GetRoleClaims(int roleId)
    {
        try
        {
            var role = await _authDbContxt.Roles.Include(x=>x.Claims).SingleAsync(x => x.Id == roleId);
            var claims = role.Claims.Select(x => new { x.Id, x.Name, x.Value });
            return Ok(claims);
        }
        catch (InvalidOperationException)
        {
            return NotFound("no role with this id");
        }
    }

    [HttpPost]
    [Route("{roleId}")]
    public async Task<IActionResult> GrantRole(TUserId userId, int roleId)
    {
        var user = await _authDbContxt.Users.FindAsync(userId);
        var role = await _authDbContxt.Roles.FindAsync(roleId);

        if (user == null)
            return NotFound("no user with this Id");

        if (role == null)
            return NotFound("no claim with this Id");

        user.Roles.Add(role);
        await _authDbContxt.SaveChangesAsync();

        return Ok("role granted");
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateRole(int id, string newName)
    {
        Role<TUser> role = new() { Id = id, Name = newName};
        _authDbContxt.Update(role);
        await _authDbContxt.SaveChangesAsync();
        return Ok(role.Id);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteClaim(int id)
    {
        await _authDbContxt.Roles.Where(x => x.Id == id).ExecuteDeleteAsync();
        return Ok();
    }
}
