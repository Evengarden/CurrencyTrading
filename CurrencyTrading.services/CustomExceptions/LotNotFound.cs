using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.services.CustomExceptions
{
    public class LotNotFound : NullReferenceException
    {
        public override string Message =>
         $"Lot not found";
    }
}
