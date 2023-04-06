using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.services.CustomExceptions
{
    public class NotEnoughBalanceForBuy : InvalidOperationException
    {
        public decimal CurrentBalance { get; init; }
        public decimal LotPrice { get; init; }
        public decimal SumOfLotForBuy { get; init; }
        public override string Message => 
            $"Cannot create lot. User RUB balance is not enough. User balance:{CurrentBalance} " +
            $", lot price:{LotPrice} , total amount of user buy lots: {SumOfLotForBuy} ";
    }
}
