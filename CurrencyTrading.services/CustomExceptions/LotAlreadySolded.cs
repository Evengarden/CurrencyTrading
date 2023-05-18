using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.services.CustomExceptions
{
    public class LotAlreadySolded : InvalidOperationException
    {
        public override string Message =>
            $"Error. Lot already solded";
    }
}
