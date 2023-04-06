using CurrencyTrading.Models;
using System.Security.Claims;

namespace CurrencyTrading.Helper
{
    public static class GetCurrentUserId
    {
        public static int GetUserId(IEnumerable<Claim> claims)
        {
            int id = int.Parse(claims.FirstOrDefault(x =>
            {
                return x.Type == "ID";
            }).Value);
            return id;
        }
    }
}
