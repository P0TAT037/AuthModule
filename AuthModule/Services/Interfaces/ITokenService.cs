using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthModule.Services.Interfaces
{
    public interface ITokenService
    {
        public JwtSecurityToken GetToken(ClaimsPrincipal claimsPrincipal);
        public string GetTokenString(IEnumerable<Claim> claims);
    }
}
