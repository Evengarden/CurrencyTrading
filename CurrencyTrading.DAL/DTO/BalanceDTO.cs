using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.DAL.DTO
{
    public class BalanceDTO
    {
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }
}
