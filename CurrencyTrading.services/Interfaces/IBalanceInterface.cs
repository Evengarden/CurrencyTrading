using CurrencyTrading.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.services.Interfaces
{
    public interface IBalanceService
    {
        Task<Balance> AddBalance(int userId,string currency,decimal amount);
        Task<ICollection<Balance>> CheckBalance(int userId);
    }
}
