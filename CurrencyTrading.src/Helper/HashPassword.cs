using System.Web.Helpers;

namespace CurrencyTrading.Helper
{
    public class HashPassword
    {
        public static string HashPass(string newPassword)
        {
            return Crypto.HashPassword(newPassword);
        }
    }
}
