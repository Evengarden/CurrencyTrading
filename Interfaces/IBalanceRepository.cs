using CurrencyTrading.Models;

namespace CurrencyTrading.Interfaces
{
    public interface IBalanceRepository
    {
        Task<Balance> CreateBalance(Balance balance);
        Task<Balance> UpdateBalance(Balance balance);
        Task<Balance> DeleteBalance(Balance balance);
        Task<bool> Save();
    }
}
