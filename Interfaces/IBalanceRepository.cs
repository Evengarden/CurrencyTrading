using CurrencyTrading.Models;

namespace CurrencyTrading.Interfaces
{
    public interface IBalanceRepository
    {
        Task<Balance> CreateBalanceAsync(Balance balance);
        Task<Balance> UpdateBalanceAsync(Balance balance);
        Task<Balance> DeleteBalanceAsync(Balance balance);
        Task<bool> SaveAsync();
    }
}
