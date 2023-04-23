using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.services.CustomExceptions
{
    public class CurrencyNotFound : InvalidOperationException
    {
        public string Currency { get; init; }
        public override string Message =>
            $"Error. Non-existed currency {Currency} ";
    }
}
