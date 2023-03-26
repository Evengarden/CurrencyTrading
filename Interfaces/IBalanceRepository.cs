using CurrencyTrading.Models;

namespace CurrencyTrading.Interfaces
{
    public interface IBalanceRepository
    {
        Balance CreateBalance(Balance balance);
        Balance UpdateBalance(Balance balance);
        Balance DeleteBalance(Balance balance);
        bool Save();

    }
}
