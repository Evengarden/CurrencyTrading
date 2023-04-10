using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.services.Helpers
{
    public static class DtoConvert
    {
        public static LotDTO LotToDto(Lot lot)
        {
            return new LotDTO
            {
                Currency = lot.Currency,
                CurrencyAmount = lot.CurrencyAmount,
                Price = lot.Price,
                Type = lot.Type
            };
        }
    }
}
