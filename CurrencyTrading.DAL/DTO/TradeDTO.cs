using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.DAL.DTO
{
    public class TradeDTO
    {
        public DateTime TradeDate { get; set; }
        public int LotId { get; set; }
        public int BuyerId { get; set; }
    }
}
