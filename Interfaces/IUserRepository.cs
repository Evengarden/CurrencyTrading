using CurrencyTrading.Models;

namespace CurrencyTrading.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<ICollection<Balance>> GetUserBalanceAsync(User user);
        Task<ICollection<Lot>> GetUserLotsAsync(User user);
        Task<ICollection<Trade>> GetUserTradesAsync(User user);
        Task<bool> SaveAsync();
    }
}
