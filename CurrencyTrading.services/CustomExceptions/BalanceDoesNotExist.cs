using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.services.CustomExceptions
{
    public class BalanceDoesNotExist : InvalidOperationException
    {
        public string Currency { get; init; }
        public string Username { get; init; }
        public override string Message =>
            $"Error. User {Username} have not currency:{Currency} on his balance ";
    }
}
