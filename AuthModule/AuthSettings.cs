using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

namespace AuthModule
{
    public class AuthSettings
    {

        public bool UseCookies { get; set; }
        public bool UseJWT { get; set; } = true;

        public JwtTokenSettings? JwtTokenSettings { get; set; }
    }

    public class JwtTokenSettings
    {
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256;
        public TimeSpan Expiration { get; set; } = TimeSpan.FromHours(1);
        public TokenValidationParameters TokenValidationParameters { get; set; } = default;
    }
}
