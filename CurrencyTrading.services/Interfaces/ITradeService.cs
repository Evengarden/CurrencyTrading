using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.services.Interfaces
{
    public interface ITradeService
    {
        Task<Trade> CreateTrade(TradeDTO tradeDTO, int userId);

        Task<Trade> GetTrade(int tradeId);
        Task<ICollection<Trade>> GetTrades();
    }
}
