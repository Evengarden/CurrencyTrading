using CurrencyTrading.Models;

namespace CurrencyTrading.Interfaces
{
    public interface ITradeRepository
    {
        Trade CreateTrade(Trade trade);
        ICollection<Trade> GetTrades();
        Trade GetTrade(int id);
        bool Save();
    }
}
