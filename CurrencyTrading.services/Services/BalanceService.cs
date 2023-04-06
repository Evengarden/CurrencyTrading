using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.services.Services
{
    public class BalanceService : IBalanceService
    {
        private readonly IBalanceRepository _balanceRepository;
        private readonly IUserRepository _userRepository;
        public BalanceService(IBalanceRepository balanceRepository,IUserRepository userRepository)
        {
            _balanceRepository = balanceRepository;
            _userRepository = userRepository;
        }
        public async Task<Balance> AddBalance(int userId,BalanceDTO balanceDTO)
        {
            var currentUserBalance = await _userRepository.GetUserAsync(userId);
            if(currentUserBalance.Balance != null)
            {
                foreach (var userBalance in currentUserBalance.Balance)
                {
                    if (userBalance.Currency == balanceDTO.Currency)
                    {
                        userBalance.Amount = balanceDTO.Amount;
                        await _userRepository.UpdateUserAsync(currentUserBalance.Id, currentUserBalance);
                        return userBalance;
                    }
                }

            }
            Balance balance = new Balance 
            {
                Currency = balanceDTO.Currency,
                Amount = balanceDTO.Amount,
                User = currentUserBalance
            };
            var newBalance = await _balanceRepository.CreateBalanceAsync(balance);
            return newBalance;
        }

        public async Task<ICollection<Balance>> CheckBalance(int userId)
        {
            var userBalance = await _userRepository.GetUserAsync(userId);
            return userBalance.Balance;
        }
    }
}
