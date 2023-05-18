using CurrencyTrading.Models;
using CurrencyTrading.services.Helpers;
using System.Security.Claims;

namespace CurrencyTrading.services.Interfaces
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
        int GetUserId(IEnumerable<Claim> claims);
        string HashPass(string newPassword);
        bool VerifyPass(string hashedPass, string pass);
    }
}
