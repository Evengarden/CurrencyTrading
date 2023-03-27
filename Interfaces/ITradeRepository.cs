using CurrencyTrading.Models;

namespace CurrencyTrading.Interfaces
{
    public interface ITradeRepository
    {
        Task<Trade> CreateTrade(Trade trade);
        Task<ICollection<Trade>> GetTrades();
        Task<Trade> GetTrade(int id);
        Task<bool> Save();
    }
}
