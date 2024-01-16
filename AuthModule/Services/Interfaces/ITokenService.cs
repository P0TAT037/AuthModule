using System.Security.Claims;

namespace AuthModule.Services.Interfaces
{
    public interface ITokenService
    {
        public string GetTokenString(IEnumerable<Claim> claims);
    }
}
