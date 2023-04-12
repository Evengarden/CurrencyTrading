using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.DAL.DTO
{
    public class CurrencyDTO
    {
        public string CurrencyCode { get; set; }
        public int CurrencyNominal { get; set; }
        public decimal CurrencyPrice { get; set; }
    }
}
