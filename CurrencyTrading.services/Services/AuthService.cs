using CurrencyTrading.Models;
using CurrencyTrading.services.Helpers;
using CurrencyTrading.services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace CurrencyTrading.services.Services
{
    public class AuthService : IAuthService
    {
        private readonly JWTSettings _config;
        public AuthService(IOptions<JWTSettings> config)
        {
            _config = config.Value;
        }

        public string GenerateJwtToken(User user)
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
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public int GetUserId(IEnumerable<Claim> claims)
        {
            int id = int.Parse(claims.FirstOrDefault(x =>
            {
                return x.Type == "ID";
            }).Value);
            return id;
        }

        public string HashPass(string newPassword)
        {
            return Crypto.HashPassword(newPassword);
        }

        public bool VerifyPass(string hashedPass, string pass)
        {
            return Crypto.VerifyHashedPassword(hashedPass, pass);
        }
    }
}
