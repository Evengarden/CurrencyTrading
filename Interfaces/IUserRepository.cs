using CurrencyTrading.Models;

namespace CurrencyTrading.Interfaces
{
    public interface IUserRepository
    {
        User CreateUser(User user);
        User UpdateUser(User user);
        ICollection<Balance> GetUserBalance(User user);
        ICollection<Lot> GetUserLots(User user);
        ICollection<Trade> GetUserTrades(User user);
        bool Save();
    }
}
