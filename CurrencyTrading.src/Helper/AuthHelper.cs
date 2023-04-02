using CurrencyTrading.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CurrencyTrading.Helper
{
    public class AuthHelper
    {
        public static string GenerateJwtToken(User user, JWTSettings _config)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim("ID", user.Id.ToString())
            };
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.SecretKey));
            var jwt = new JwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                claims: claims,
                signingCredentials: new SigningCredentials(signingKey,SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
