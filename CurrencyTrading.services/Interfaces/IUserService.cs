using CurrencyTrading.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.services.Interfaces
{
    public interface IUserService
    {
        Task<string?> Auth(User user);
        Task UserRegistration(User user);
        Task<User> GetCurrentUser(int userId);
    }
}
