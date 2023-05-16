﻿using AutoMapper;
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
        private readonly IBalanceCalculationService _balanceCalculationService;

        public TradeService(ITradeRepository tradeRepository, IUserRepository userRepository,
            ILotRepository lotRepository,  IBalanceCalculationService balanceCalculationService)
        {
            _tradeRepository = tradeRepository;
            _userRepository = userRepository;
            _lotRepository = lotRepository;
            _balanceCalculationService = balanceCalculationService;
        }
        public async Task<Trade> CreateTrade(TradeDTO tradeDTO, int userId)
        {
            var user = await _userRepository.GetUserAsync(userId);
            var lot = await _lotRepository.GetLotAsync(tradeDTO.LotId);
            if (lot is null)
            {
                throw new LotNotFound();
            }
            if (lot.Type == Types.Sold)
            {
                _balanceCalculationService.CheckEnoughBalanceForBuy(user, lot);
            }
            else
            {
                _balanceCalculationService.CheckEnoughBalanceForSold(user, lot);
            }
            var updatedBalanceUser = await _balanceCalculationService.CalculateBalance(user, lot);
            var trade = new Trade
            {
                TradeDate = DateTime.Now,
                LotId = tradeDTO.LotId,
                BuyerId = user.Id,
                Buyer = updatedBalanceUser,
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
