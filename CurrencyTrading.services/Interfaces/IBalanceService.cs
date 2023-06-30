using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Models;

namespace CurrencyTrading.services.Interfaces
{
    public interface IBalanceService
    {
        Task<Balance> AddBalance(int userId,BalanceDTO balanceDTO);
        Task<ICollection<Balance>> CheckBalance(int userId);
    }
}
