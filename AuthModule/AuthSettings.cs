using AuthModule.Data;
using AuthModule.Data.Models.Abstract;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

namespace AuthModule;

public class AuthSettings<TUser, TUserId>
    where TUser : class, IUser<TUser, TUserId>
{
    private readonly List<PropertyInfo> userInfoClaims = new List<PropertyInfo>();
    
    public delegate DbContextOptionsBuilder DbOptionsBuilder(DbContextOptionsBuilder optionsBuilder);

    public bool UseCookies { get; set; }

    public JwtTokenSettings? JwtTokenSettings { get; set; } = new();

    public CookieSettings? CookieSettings { get; set; } = new();

    public required DbOptionsBuilder ConfigureDbOptions { get; set; }

    public Action<AuthDbContxt<TUser, TUserId>>? AuthDbInitializer { get; set; }
    
    public IEnumerable<PropertyInfo> UserInfoClaims => userInfoClaims;

    public AuthSettings<TUser, TUserId> AddUserInfoClaim(string UserModelPropertyName)
    {
        var property = typeof(TUser).GetProperty(UserModelPropertyName)!;

        if (property == null)
            throw new Exception("no property with this name");

        userInfoClaims.Add(property);

        return this;
    }
}

public class JwtTokenSettings
{
    public delegate void D(out JwtBearerOptions options);

    public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256;
    
    public TimeSpan Expiration { get; set; } = TimeSpan.FromHours(1);

    public JwtBearerOptions? Options { get; private set; }

    public Action<JwtBearerOptions>? ConfigOptions { get; set; }

    internal void ConfigureJwtBearerOptions(JwtBearerOptions options)
    {
        if (ConfigOptions != null)
            ConfigOptions(options);
        
        Options = options;
    }
}

public class CookieSettings
{
    public CookieAuthenticationOptions? Options { get; private set; }
    public Action<CookieAuthenticationOptions>? ConfigOptions { get; set; }
    internal void ConfigureCookieAuthenticationOptions(CookieAuthenticationOptions options) 
    {
        if(ConfigOptions != null)
            ConfigOptions(options);
        
        Options = options;
    }

}