using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Models;
using CurrencyTrading.services.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.services.Helpers
{
    public class CheckBalances
    {
        public static void CheckEnoughBalanceForSold(User user, List<Lot> userLots, LotDTO lot)
        {
            var userBalance = user.Balance.FirstOrDefault(b =>
            {
                return b.Currency == lot.Currency;
            });
            decimal userLotsAmountSum = userLots.Where(u => u.Currency == lot.Currency &&
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
        }
        public static void CheckEnoughBalanceForBuy(User user, List<Lot> userLots, LotDTO lot)
        {
            var userBalance = user.Balance.FirstOrDefault(b =>
            {
                return b.Currency == "RUB";
            });
            decimal userLotsSumOfBuy = userLots.Where(u => u.Type == Types.Buy && 
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
        }
    }
}
