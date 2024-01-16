using AuthModule.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthModule.Services
{
    public class JwtTokenService : ITokenService
    {

        public JwtTokenSettings JwtTokenSettings { get; set; }

        public JwtTokenService(IOptionsSnapshot<AuthSettings> config)
        {
            JwtTokenSettings = config.Value.JwtTokenSettings!;
        }

        //key sample: HrafCOb3jt045IBZn1Z6RPUAxDkavf_INZzE9BwN3I0cQzuElDShtNCSXub5Ef7JazFot3iCJ3UBpIbIrHbtzA
        public JwtSecurityToken GetToken(ClaimsPrincipal claimsPrincipal)
        {
            var credentials = new SigningCredentials(
                JwtTokenSettings.TokenValidationParameters!.IssuerSigningKey,
                JwtTokenSettings.SecurityAlgorithm);

            return new JwtSecurityToken(
                issuer: JwtTokenSettings.TokenValidationParameters.ValidIssuer,
                audience: JwtTokenSettings.TokenValidationParameters.ValidAudience,
                claimsPrincipal.Claims,
                expires: DateTime.Now.Add(JwtTokenSettings.Expiration),
                signingCredentials: credentials
                );
        }
        public JwtSecurityToken GetToken(IEnumerable<Claim> claims)
        {
            var credentials = new SigningCredentials(
                JwtTokenSettings.TokenValidationParameters!.IssuerSigningKey,
                JwtTokenSettings.SecurityAlgorithm);

            return new JwtSecurityToken(
                issuer: JwtTokenSettings.TokenValidationParameters.ValidIssuer,
                audience: JwtTokenSettings.TokenValidationParameters.ValidAudience,
                claims,
                expires: DateTime.Now.Add(JwtTokenSettings.Expiration),
                signingCredentials: credentials
                );
        }

        public string GetTokenString(IEnumerable<Claim> claims)
        {
            var token = GetToken(claims);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
