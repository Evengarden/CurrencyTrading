using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Models;

namespace CurrencyTrading.services.Interfaces
{
    public interface IUserService
    {
        Task<string?> Auth(UserDTO user);
        Task UserRegistration(User user);
        Task<User> GetCurrentUser(int userId);
        Task<User> UpdateUser(int userId,UserDTO user);
    }
}
