using AutoMapper;
using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.services.CustomExceptions;
using CurrencyTrading.services.Helpers;
using CurrencyTrading.services.Interfaces;

namespace CurrencyTrading.services.Services
{
    public class TradeService : ITradeService
    {
        private readonly ITradeRepository _tradeRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILotRepository _lotRepository;
        private readonly IMapper _mapper;

        public TradeService(ITradeRepository tradeRepository, IUserRepository userRepository, ILotRepository lotRepository, IMapper mapper)
        {
            _tradeRepository = tradeRepository;
            _userRepository = userRepository;
            _lotRepository = lotRepository;
            _mapper = mapper;

        }
        public async Task<Trade> CreateTrade(TradeDTO tradeDTO, int userId)
        {
            var user = await _userRepository.GetUserAsync(userId);
            var lot = await _lotRepository.GetLotAsync(tradeDTO.LotId);
            if(lot is null)
            {
                throw new LotNotFound();
            }
            var owner = await _userRepository.GetUserAsync(lot.Owner.Id);
            if (user.Id == owner.Id)
            {
                throw new OwnerIsBuyer();
            }

            var userBalance = user.Balance.FirstOrDefault(b => b.Currency == lot.Currency);
            var mainUserBalance = user.Balance.FirstOrDefault(b => b.Currency == "RUB");

            var ownerBalance = owner.Balance.FirstOrDefault(b => b.Currency == lot.Currency);
            var mainOwnerBalance = owner.Balance.FirstOrDefault(b => b.Currency == "RUB");

            var userLots = user.Lots.ToList();
            if (lot.Type == Types.Sold)
            {
                CheckBalances.CheckEnoughBalanceForBuy(user, userLots,_mapper.Map<LotDTO>(lot));
                userBalance.Amount = userBalance.Amount + lot.CurrencyAmount;
                mainUserBalance.Amount = mainUserBalance.Amount - lot.Price;

               
                ownerBalance.Amount = ownerBalance.Amount - lot.CurrencyAmount;
                mainOwnerBalance.Amount = mainOwnerBalance.Amount + lot.Price;
            }
            else
            {
                CheckBalances.CheckEnoughBalanceForSold(user, userLots, _mapper.Map<LotDTO>(lot));

                userBalance.Amount = userBalance.Amount - lot.CurrencyAmount;
                mainUserBalance.Amount = mainUserBalance.Amount + lot.Price;


                ownerBalance.Amount = ownerBalance.Amount + lot.CurrencyAmount;
                mainOwnerBalance.Amount = mainOwnerBalance.Amount - lot.Price;
            }
            var trade = new Trade
            {
                TradeDate = DateTime.Now,
                LotId = tradeDTO.LotId,
                BuyerId = user.Id,
                Buyer = user,
                TradeLot = lot
            };
            var createdTrade = await _tradeRepository.CreateTradeAsync(trade);
            lot.Status = Statuses.Solded;
            await _lotRepository.UpdateLotAsync(lot.Id, lot);
            return createdTrade;
        }

        public async Task<Trade> GetTrade(int tradeId)
        {
            var trade = await _tradeRepository.GetTradeAsync(tradeId);
            if (trade is null)
            {
                throw new TradeNotFound();
            } 
            return trade;
        }

        public async Task<ICollection<Trade>> GetTrades()
        {
            var trades = await _tradeRepository.GetTradesAsync();
            return trades;
        }
    }
}
