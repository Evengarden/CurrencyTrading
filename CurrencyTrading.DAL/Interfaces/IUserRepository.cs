using CurrencyTrading.Models;

namespace CurrencyTrading.Interfaces
{
    public interface IUserRepository
    {

        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(int userId,User user);
        Task<User> GetUserAsync(int userId);
        Task<User> CheckCredentails(string login,string password);
    }
}
