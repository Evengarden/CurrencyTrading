using System.Web.Helpers;

namespace CurrencyTrading.Helper
{
    public class HashPassword
    {
        public static string HashPass(string newPassword)
        {
            return Crypto.HashPassword(newPassword);
        }

        public static bool VerifyPass(string hashedPass,string pass)
        {
            return Crypto.VerifyHashedPassword(hashedPass, pass);
        }
    }
}
