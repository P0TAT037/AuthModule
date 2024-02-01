using AuthModule.Data;
using AuthModule.Data.Models;
using AuthModule.Data.Models.Abstract;
using AuthModule.DTOs;
using AuthModule.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthModule.Controllers;

[ApiController]
[Route("api/auth/claim")]
[Authorize(AuthModuleConstValues.AdminPolicy)]
public class ClaimsController<TUser, TUserId> : ControllerBase
   where TUser : class, IUser<TUser, TUserId>
{
    private readonly AuthDbContxt<TUser, TUserId> _authDbContxt;
    private readonly AuthSettings<TUser, TUserId> _authSettings;
    public ClaimsController(AuthDbContxt<TUser, TUserId> authDbContxt, AuthSettings<TUser, TUserId> authSettings)
    {
        _authDbContxt = authDbContxt;
        _authSettings = authSettings;
    }

    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetClaims()
    {
        return Ok(await _authDbContxt.Claims.Select(x => new { x.Id, x.Name, x.Value }).ToListAsync());
    }

    [HttpGet]
    public async Task<IActionResult> GetUserClaims(TUserId userId)
    {
        TUser user;
        try
        {
            user = await _authDbContxt.Users.Include(x => x.Claims).SingleAsync(x => x.Id.Equals(userId));
        }
        catch (InvalidOperationException)
        {
            return NotFound("no user with this id");
        }

        return Ok(user.Claims.Select(x => new { x.Id, x.Name, x.Value }));
    }

    [HttpPost]
    public async Task<IActionResult> AddClaims(List<ClaimDto> claimDtos)
    {
        List<Claim<TUser>> claims = claimDtos.Select(x => new Claim<TUser>() { Name = x.Name, Value = x.Value }).ToList();

        await _authDbContxt.Claims.AddRangeAsync(claims);
        await _authDbContxt.SaveChangesAsync();

        return Ok();
    }

    [HttpPost]
    [Route("{claimId}")]
    public async Task<IActionResult> GrantClaim(TUserId userId, int claimId)
    {
        var user = await _authDbContxt.Users.FindAsync(userId);
        var claim = await _authDbContxt.Claims.FindAsync(claimId);

        if (user == null)
            return NotFound("no user with this Id");

        if (claim == null)
            return NotFound("no claim with this Id");

        user.Claims.Add(claim);
        await _authDbContxt.SaveChangesAsync();

        return Ok("claim granted");
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateClaim(int id, ClaimDto claimDto)
    {
        Claim<TUser> claim = new() { Id = id, Name = claimDto.Name, Value = claimDto.Value };
        _authDbContxt.Update(claim);
        await _authDbContxt.SaveChangesAsync();
        return Ok(claim.Id);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteClaim(int id)
    {
        await _authDbContxt.Claims.Where(x => x.Id == id).ExecuteDeleteAsync();
        return Ok();
    }
}
