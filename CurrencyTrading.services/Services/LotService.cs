using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.services.CustomExceptions;
using CurrencyTrading.services.Interfaces;
using System.Runtime.CompilerServices;
using System.Transactions;
using CurrencyTrading.services.Helpers;

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
                CheckBalances.CheckEnoughBalanceForSold(user, userLots, lot);
            }
            else
            {
                CheckBalances.CheckEnoughBalanceForBuy(user, userLots, lot);
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
                CheckBalances.CheckEnoughBalanceForSold(user, userLots, lot);
            }
            else
            {
                CheckBalances.CheckEnoughBalanceForBuy(user, userLots, lot);
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

       
    }
}
