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

        private readonly JwtTokenSettings _jwtTokenSettings;

        public JwtTokenService(JwtTokenSettings jwtTokenSettings)
        {
            _jwtTokenSettings = jwtTokenSettings!;
        }

        //key sample: HrafCOb3jt045IBZn1Z6RPUAxDkavf_INZzE9BwN3I0cQzuElDShtNCSXub5Ef7JazFot3iCJ3UBpIbIrHbtzA
        public JwtSecurityToken GetToken(ClaimsPrincipal claimsPrincipal)
        {
            var credentials = new SigningCredentials(
                _jwtTokenSettings.Options.TokenValidationParameters!.IssuerSigningKey,
                _jwtTokenSettings.SecurityAlgorithm);

            return new JwtSecurityToken(
                issuer: _jwtTokenSettings.Options.TokenValidationParameters.ValidIssuer,
                audience: _jwtTokenSettings.Options.TokenValidationParameters.ValidAudience,
                claimsPrincipal.Claims,
                expires: DateTime.Now.Add(_jwtTokenSettings.Expiration),
                signingCredentials: credentials
                );
        }
        public JwtSecurityToken GetToken(IEnumerable<Claim> claims)
        {
            var credentials = new SigningCredentials(
                _jwtTokenSettings.Options.TokenValidationParameters!.IssuerSigningKey,
                _jwtTokenSettings.SecurityAlgorithm);

            return new JwtSecurityToken(
                issuer: _jwtTokenSettings.Options.TokenValidationParameters.ValidIssuer,
                audience: _jwtTokenSettings.Options.TokenValidationParameters.ValidAudience,
                claims,
                expires: DateTime.Now.Add(_jwtTokenSettings.Expiration),
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
