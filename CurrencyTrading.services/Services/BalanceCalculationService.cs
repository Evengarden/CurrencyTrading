using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.services.CustomExceptions;
using CurrencyTrading.services.Helpers;
using CurrencyTrading.services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.services.Services
{
    public class BalanceCalculationService : IBalanceCalculationService
    {
        private readonly IBalanceRepository _balanceRepository;
        public BalanceCalculationService(IBalanceRepository balanceRepository)
        {
            _balanceRepository = balanceRepository;
        }

        public async Task<User> CalculateBalance(User buyer, User lotOwner, Lot lot)
        {
            if (buyer.Id == lot.Owner.Id)
            {
                throw new OwnerIsBuyer();
            }
            var userBalance = buyer.Balance.SingleOrDefault(b => b.Currency == lot.Currency);
            if(userBalance is null)
            {
                Balance balance = new Balance
                {
                    Currency = lot.Currency,
                    Amount = 0,
                    User = buyer
                };
                userBalance = await _balanceRepository.CreateBalanceAsync(balance);
                buyer.Balance.Add(userBalance);
            }
            var mainUserBalance = buyer.Balance.SingleOrDefault(b => b.Currency == "RUB");
            var ownerBalance = lotOwner.Balance.SingleOrDefault(b => b.Currency == lot.Currency);
            var mainOwnerBalance = lot.Owner?.Balance?.SingleOrDefault(b => b.Currency == "RUB");
            if (lot.Type == Types.Sold)
            {
                userBalance.Amount = userBalance.Amount + lot.CurrencyAmount;
                mainUserBalance.Amount = mainUserBalance.Amount - lot.Price;

                ownerBalance.Amount = ownerBalance.Amount - lot.CurrencyAmount;
                mainOwnerBalance.Amount = mainOwnerBalance.Amount + lot.Price;

            }
            else
            {
                userBalance.Amount = userBalance.Amount - lot.CurrencyAmount;
                mainUserBalance.Amount = mainUserBalance.Amount + lot.Price;

                ownerBalance.Amount = ownerBalance.Amount + lot.CurrencyAmount;
                mainOwnerBalance.Amount = mainOwnerBalance.Amount - lot.Price;

            }
            return buyer;
        }
        public Balance CheckEnoughBalanceForBuy(User user, Lot lot)
        {
            var userBalance = user.Balance.FirstOrDefault(b =>
            {
                return b.Currency == "RUB";
            });
            decimal userLotsSumOfBuy = user.Lots.Where(u => u.Type == Types.Buy &&
                                                      u.Status != Statuses.Solded)
                                                      .Sum(l => l.Price);
            if (userBalance != null)
            {
                if (userBalance.Amount < lot.Price || userBalance.Amount < lot.Price + userLotsSumOfBuy)
                {
                    throw new NotEnoughBalanceForBuy
                    {
                        CurrentBalance = userBalance.Amount,
                        LotPrice = lot.Price,
                        SumOfLotForBuy = lot.Price + userLotsSumOfBuy
                    };
                }
            }
            else
            {
                throw new BalanceDoesNotExist
                {
                    Currency = lot.Currency,
                    Username = user.Login
                };
            }
            return userBalance;
        }

        public Balance CheckEnoughBalanceForSold(User user, Lot lot)
        {
            var userBalance = user.Balance.FirstOrDefault(b =>
            {
                return b.Currency == lot.Currency;
            });
            decimal userLotsAmountSum = user.Lots.Where(u => u.Currency == lot.Currency &&
                                                        u.Type == Types.Sold && u.Status != Statuses.Solded)
                                                .Sum(l => l.CurrencyAmount);
            if (userBalance != null)
            {
                if (userBalance.Amount < lot.CurrencyAmount || userBalance.Amount < userLotsAmountSum + lot.CurrencyAmount)
                {
                    throw new NotEnoughBalanceForSold
                    {
                        CurrentBalance = userBalance.Amount,
                        LotCurrencyAmount = lot.CurrencyAmount,
                        UserLotAmountSum = userLotsAmountSum + lot.CurrencyAmount
                    };
                }
            }
            else
            {
                throw new BalanceDoesNotExist
                {
                    Currency = lot.Currency,
                    Username = user.Login
                };
            }
            return userBalance;
        }
    }
}
