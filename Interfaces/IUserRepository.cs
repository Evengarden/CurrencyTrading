using CurrencyTrading.Models;

namespace CurrencyTrading.Interfaces
{
    public interface IUserRepository
    {

        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<User> GetUser(int userId);
        Task<bool> SaveAsync();
    }
}
