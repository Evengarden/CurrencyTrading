﻿using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.Repository;
using CurrencyTrading.services.Helpers;
using CurrencyTrading.services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.services.Services
{
    public class TradeService : ITradeService
    {
        public readonly ITradeRepository _tradeRepository;
        public readonly IUserRepository _userRepository;
        public readonly ILotRepository _lotRepository;
        public TradeService(ITradeRepository tradeRepository, IUserRepository userRepository, ILotRepository lotRepository)
        {
            _tradeRepository = tradeRepository;
            _userRepository = userRepository;
            _lotRepository = lotRepository;

        }
        public async Task<Trade> CreateTrade(TradeDTO tradeDTO, int userId)
        {
            var user = await _userRepository.GetUserAsync(userId);
            var lot = await _lotRepository.GetLotAsync(tradeDTO.LotId);
            var owner = await _userRepository.GetUserAsync(lot.Owner.Id);


            var userBalance = user.Balance.FirstOrDefault(b => b.Currency == lot.Currency);
            var mainUserBalance = user.Balance.FirstOrDefault(b => b.Currency == "RUB");

            var ownerBalance = owner.Balance.FirstOrDefault(b => b.Currency == lot.Currency);
            var mainOwnerBalance = owner.Balance.FirstOrDefault(b => b.Currency == "RUB");

            var userLots = user.Lots.ToList();
            if (lot.Type == Types.Sold)
            {
                CheckBalances.CheckEnoughBalanceForBuy(user, userLots, DtoConvert.LotToDto(lot));
               
                userBalance.Amount = userBalance.Amount + lot.CurrencyAmount;
                mainUserBalance.Amount = mainUserBalance.Amount - lot.Price;

               
                ownerBalance.Amount = ownerBalance.Amount - lot.CurrencyAmount;
                mainOwnerBalance.Amount = mainOwnerBalance.Amount + lot.Price;
            }
            else
            {
                CheckBalances.CheckEnoughBalanceForSold(user, userLots, DtoConvert.LotToDto(lot));

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
            return trade;
        }

        public async Task<ICollection<Trade>> GetTrades()
        {
            var trades = await _tradeRepository.GetTradesAsync();
            return trades;
        }
    }
}