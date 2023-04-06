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
        public override string Message =>
            $"Cannot create sold lot. User have not currency:{Currency} on his balance ";
    }
}
