using AuthModule.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using AuthModule.Data.Models.Abstract;
using AuthModule.DTOs.Abstract;
using AuthModule.Services.Interfaces;
using System.Security.Claims;
using AuthModule.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace AuthModule.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController<TUser, TUserRegistrationDto, TUserId> : ControllerBase
    where TUser : class, IUser<TUser, TUserId>
    where TUserRegistrationDto : class, IUserDto
{
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly AuthDbContxt<TUser, TUserId> _authDbContext;
    private readonly AuthSettings<TUser, TUserId> _authSettings;

    public AuthController(IMapper mapper, AuthDbContxt<TUser, TUserId> authDbContext, ITokenService tokenService, AuthSettings<TUser, TUserId> authSettings)
    {
        _mapper = mapper;
        _authDbContext = authDbContext;
        _tokenService = tokenService;
        _authSettings = authSettings;

    }


    [HttpPost]
    [Route("signup")]
    public async Task<IActionResult> SignUp(TUserRegistrationDto user)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        var u = _mapper.Map<TUser>(user);
        _authDbContext.Users.Add(u);
        await _authDbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPost]
    [Route("signin")]
    public async Task<IActionResult> Login([FromForm] string Handle, [FromForm] string Password)
    {
        TUser user;

        try
        {
            user = _authDbContext.Users.Single(x => x.Handle == Handle);

            if (!BCrypt.Net.BCrypt.Verify(Password, user.Password))
                throw new InvalidOperationException("wrong password");
        }
        catch (InvalidOperationException)
        {
            return Unauthorized("wrong username or password");
        }



        await _authDbContext.Entry(user).Collection(x => x.Roles).LoadAsync();
        await _authDbContext.Entry(user).Collection(x => x.Claims).LoadAsync();

        var claims = new List<Claim>() { new(ClaimTypes.NameIdentifier, user.Id!.ToString()!) };

        foreach (var item in _authSettings.UserInfoClaims)
        {
            string name, value;
            try
            {
                name = item.Name.Split('.').Last();
                value = item.GetValue(user)!.ToString()!;
            }
            catch
            {
                throw new InvalidOperationException("unable to parse UserInfoClaims");
            }

            claims.Add(new(name, value));
        }

        var roles = new List<Claim<TUser>>();
        foreach (var role in user.Roles)
        {
            roles.Add(role.GetClaim());
        }

        var allClaims = roles.Concat(user.Claims); //Claims of our custom claim type

        foreach (var claim in allClaims)
        {
            claims.Add(new(claim.Name, claim.Value));
        }

        if (_authSettings.UseCookies)
        {
            var principle = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);
            return SignIn(principle);
        }

        var token = _tokenService.GetTokenString(claims);
        return Ok(token);
    }

}
