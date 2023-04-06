using CurrencyTrading.DAL.DTO;
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
        Task<Balance> AddBalance(int userId,BalanceDTO balanceDTO);
        Task<ICollection<Balance>> CheckBalance(int userId);
    }
}
