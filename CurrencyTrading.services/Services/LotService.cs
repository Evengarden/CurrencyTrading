using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.services.CustomExceptions;
using CurrencyTrading.services.Interfaces;
using CurrencyTrading.DAL.Helpers;
using AutoMapper;

namespace CurrencyTrading.services.Services
{
    public class LotService : ILotService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILotRepository _lotRepository;
        private readonly IBalanceCalculationService _balanceCalculationService;
        private readonly IMapper _mapper;
        public LotService(IUserRepository userRepository, ILotRepository lotRepository, 
            IBalanceCalculationService balanceCalculationService, IMapper mapper)
        {
            _userRepository = userRepository;
            _lotRepository = lotRepository;
            _balanceCalculationService = balanceCalculationService;
            _mapper = mapper;
        }

        public async Task<Lot> CreateLot(int userId, LotDTO lot)
        {
            var user = await _userRepository.GetUserAsync(userId);

            if (lot.Type == Types.Sold)
            {
                _balanceCalculationService.CheckEnoughBalanceForSold(user, _mapper.Map<Lot>(lot));
            }
            else
            {
                _balanceCalculationService.CheckEnoughBalanceForBuy(user, _mapper.Map<Lot>(lot));
            }
            Lot newLot = new Lot 
            {
                Currency = lot.Currency,
                Automatch = lot.Automatch,
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
            if(currentLot is null)
            {
                throw new LotNotFound();
            }
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
            var lot = await _lotRepository.GetLotAsync(lotId);
            if (lot is null)
            {
                throw new LotNotFound();
            }
            return lot;
        }

        public async Task<ICollection<Lot>> GetLots()
        {
            return await _lotRepository.GetLotsAsync();
        }

        public async Task<Lot> UpdateLot(int lotId,LotDTO lot,int userId)
        {
            var updatedLot = await _lotRepository.GetLotAsync(lotId);

            if (updatedLot is null)
            {
                throw new LotNotFound();
            }

            var user = await _userRepository.GetUserAsync(userId);
            if (lot.Type == Types.Sold)
            {
                _balanceCalculationService.CheckEnoughBalanceForSold(user, _mapper.Map<Lot>(lot));
            }
            else
            {
                _balanceCalculationService.CheckEnoughBalanceForBuy(user, _mapper.Map<Lot>(lot));
            }

            var newLot = UpdateEntityHelper.updateEntity(lot,updatedLot);
            if (updatedLot.Status == Statuses.Solded) 
            {
                throw new LotAlreadySolded();
            }

            return await _lotRepository.UpdateLotAsync(lotId, newLot);
        }

       
    }
}
