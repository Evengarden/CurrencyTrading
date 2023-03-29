using CurrencyTrading.Models;

namespace CurrencyTrading.Interfaces
{
    public interface ITradeRepository
    {
        Task<Trade> CreateTradeAsync(Trade trade);
        Task<ICollection<Trade>> GetTradesAsync();
        Task<Trade> GetTradeAsync(int id);
        Task<bool> SaveAsync();
    }
}
