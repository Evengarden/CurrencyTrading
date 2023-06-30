using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Models;

namespace CurrencyTrading.services.Interfaces
{
    public interface ITradeService
    {
        Task<Trade> CreateTrade(TradeDTO tradeDTO, int userId);

        Task<Trade> GetTrade(int tradeId);
        Task<ICollection<Trade>> GetTrades();
    }
}
