using AuthModule.Data;
using AuthModule.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using AuthModule.Data.Models.Abstract;
using AuthModule.DTOs.Abstract;
using AuthModule.Services.Interfaces;
using System.Security.Claims;
using AuthModule.Data.Models;

namespace AuthModule.Controllers.Abstract;

[ApiController]
[Route("api/auth")]
public abstract class Auth<TUser, TUserRegistrationDto, TUserId> : ControllerBase
    where TUser : class, IUser<TUser, TUserId>
    where TUserRegistrationDto : class, IUserDto
{
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly AuthDbContxt<TUser, TUserId> _authDbContxt;

    public Auth(IMapper mapper, AuthDbContxt<TUser, TUserId> authDbContxt, ITokenService tokenService)
    {
        _mapper = mapper;
        _authDbContxt = authDbContxt;
        _tokenService = tokenService;
    }


    [HttpPost]
    [Route("signup")]
    public async Task<IActionResult> SignUp(TUserRegistrationDto user)
    {
        user.Password  = BCrypt.Net.BCrypt.HashPassword(user.Password);
        var u = _mapper.Map<TUser>(user);
        _authDbContxt.Users.Add(u);
        await _authDbContxt.SaveChangesAsync();
        return Ok();
    }

    [HttpPost]
    [Route("signin")]
    public async Task<IActionResult> Login([FromForm] string Handle, [FromForm] string Password)
    {
        TUser user;
        
        try
        {
            user = _authDbContxt.Users.Single(x => x.Handle == Handle);

            if (!BCrypt.Net.BCrypt.Verify(Password, user.Password))
                throw new InvalidOperationException("wrong password");
        }
        catch (InvalidOperationException)
        {
            return Unauthorized("wrong username or password");
        }

        await _authDbContxt.Entry(user).Collection(x=>x.Claims).LoadAsync();
        var claims = new List<Claim>() { new(ClaimTypes.NameIdentifier, user.Id.ToString()) };
        var roleClaims = new List<Claim<TUser>>();
        foreach (var role in user.Roles)
        {
            roleClaims.Add(role.GetClaim());
        }
        var allClaims = roleClaims.Concat(user.Claims); //Claims of our custom claim type

        foreach (var claim in allClaims)
        {
            claims.Add(new(claim.Name, claim.Value));
        }
        
        var token = _tokenService.GetTokenString(claims);
        return Ok(token);
    }

}
