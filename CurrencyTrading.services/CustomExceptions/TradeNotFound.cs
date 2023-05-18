using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.services.CustomExceptions
{
    public class TradeNotFound : NullReferenceException
    {
        public override string Message =>
         $"Trade not found";
    }
}
