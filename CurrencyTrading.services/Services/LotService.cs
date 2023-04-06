using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.services.CustomExceptions;
using CurrencyTrading.services.Interfaces;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace CurrencyTrading.services.Services
{
    public class LotService : ILotService
    {
        private readonly IBalanceRepository _balanceRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILotRepository _lotRepository;
        public LotService(IBalanceRepository balanceRepository, IUserRepository userRepository, ILotRepository lotRepository)
        {
            _balanceRepository = balanceRepository;
            _userRepository = userRepository;
            _lotRepository = lotRepository;
        }
        public async Task<Lot> CreateLot(int userId, LotDTO lot)
        {
            var user = await _userRepository.GetUserAsync(userId);
            var userLots = user.Lots.ToList();

            if (lot.Type == Types.Sold)
            {
                CheckEnoughBalanceForSold(user, userLots, lot);
            }
            else
            {
                CheckEnoughBalanceForBuy(user, userLots, lot);
            }
            Lot newLot = new Lot 
            {
                Currency = lot.Currency,
                CurrencyAmount = lot.CurrencyAmount,
                Price = lot.Price,
                Owner = user,
                Status = Statuses.Created,
                Type = lot.Type
            };
            var createdLot = await _lotRepository.CreateLotAsync(newLot);
            return createdLot;
        }

        public async Task<Lot> DeleteLot(int lotId)
        {
            var currentLot = await _lotRepository.GetLotAsync(lotId);
            if (currentLot.Status != Statuses.Solded)
            {
                var lot = await _lotRepository.DeleteLotAsync(lotId);
                return lot;
            }
            else
            {
                throw new LotAlreadySolded();
            }
        }

        public async Task<Lot> GetLot(int lotId)
        {
            return await _lotRepository.GetLotAsync(lotId);
        }

        public async Task<ICollection<Lot>> GetLots(int userId)
        {
            return await _lotRepository.GetLotsAsync();
        }

        public async Task<Lot> UpdateLot(int lotId,LotDTO lot,int userId)
        {
            var user = await _userRepository.GetUserAsync(userId);
            var userLots = user.Lots.ToList();
            if (lot.Type == Types.Sold)
            {
                CheckEnoughBalanceForSold(user, userLots, lot);
            }
            else
            {
                CheckEnoughBalanceForBuy(user, userLots, lot);
            }
            var updatedLot = await _lotRepository.GetLotAsync(lotId);
            if (updatedLot.Status == Statuses.Solded) 
            {
                throw new LotAlreadySolded();
            }
            if (updatedLot.Currency != lot.Currency)
            {
                updatedLot.Currency = lot.Currency;
            }
            if (updatedLot.CurrencyAmount != lot.CurrencyAmount)
            {
                updatedLot.CurrencyAmount = lot.CurrencyAmount;
            }
            if (updatedLot.Price != lot.Price)
            {
                updatedLot.Price = lot.Price;
            }
           
            return await _lotRepository.UpdateLotAsync(lotId,updatedLot);
        }

        private void CheckEnoughBalanceForSold(User user,List<Lot> userLots, LotDTO lot)
        {
           var userBalance = user.Balance.FirstOrDefault(b =>
            {
                return b.Currency == lot.Currency;
            });
            decimal userLotsAmountSum = userLots.Where(u=>u.Currency == lot.Currency &&
                                                        u.Type == Types.Sold)
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
                    Currency = lot.Currency
                };
            }
        }
        private void CheckEnoughBalanceForBuy(User user, List<Lot> userLots, LotDTO lot)
        {
            var userBalance = user.Balance.FirstOrDefault(b =>
            {
                return b.Currency == "RUB";
            });
            decimal userLotsSumOfBuy = userLots.Where(u=>u.Type == Types.Buy)
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
        }
    }
}
