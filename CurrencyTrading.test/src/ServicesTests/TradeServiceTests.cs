using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Data;
using CurrencyTrading.Helper;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.services.CustomExceptions;
using CurrencyTrading.services.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.test.src.ServicesTests
{
    public class TradeServiceTests
    {
        private readonly TradeService _tradeService;
        private readonly DataContext _ctx;
        private readonly User _user;
        private readonly User _user2;
        private readonly Lot _lot;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<ITradeRepository> _tradeRepository;
        private readonly Mock<ILotRepository> _lotRepository;

        public TradeServiceTests()
        {
            var dbOptions = new DbContextOptionsBuilder<DataContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            _ctx = new DataContext(dbOptions);
            _ctx.Database.EnsureCreated();
            _user = _ctx.Users.Add(new User
            {
                Login = "test",
                Password = HashPassword.HashPass("test")
            }).Entity;

            _user2 = _ctx.Users.Add(new User
            {
                Login = "test1",
                Password = HashPassword.HashPass("test1")
            }).Entity;
            _user.Balance = new List<Balance> {
                new Balance {
                Amount = 100,
                Currency ="USD",
                User = _user
                },
                new Balance {
                Amount = 10000,
                Currency ="RUB",
                User = _user
                }
            };
            _user2.Balance = new List<Balance> {
                new Balance {
                Amount = 100,
                Currency ="USD",
                User = _user2
                },
                new Balance {
                Amount = 10000,
                Currency ="RUB",
                User = _user2
                }
            };
            _lot = _ctx.Lots.Add(new Lot
            {
                Currency = "USD",
                CurrencyAmount = 10,
                Status = Statuses.Created,
                Price = 100,
                OwnerId = _user.Id,
                Type = Types.Sold
            }).Entity;
            _tradeRepository = new Mock<ITradeRepository>();
            _lotRepository = new Mock<ILotRepository>();
            _userRepository = new Mock<IUserRepository>();
            _tradeService = new TradeService(_tradeRepository.Object,_userRepository.Object,_lotRepository.Object);
        }

        [Fact]
        public async Task CreateTrade_ShouldReturnCreatedTradeForSoldLot()
        {
            //arrange
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _userRepository.Setup(u => u.GetUserAsync(_user2.Id)).ReturnsAsync(_user2);
            _lotRepository.Setup(l=>l.GetLotAsync(_lot.Id)).ReturnsAsync(_lot);
            TradeDTO tradeDTO = new TradeDTO
            {
                LotId=_lot.Id
            };
            Trade trade = new Trade
            {
                Buyer = _user2,
                TradeLot=_lot,
                LotId = tradeDTO.LotId,
                BuyerId = _user2.Id,
                TradeDate = DateTime.Now,
            };
            _tradeRepository.Setup(t => t.CreateTradeAsync(It.IsAny<Trade>())).ReturnsAsync(trade);
            _lotRepository.Setup(l => l.UpdateLotAsync(_lot.Id,_lot)).ReturnsAsync(_lot);
            //act
            var result = await _tradeService.CreateTrade(tradeDTO, _user2.Id);
            //assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateTrade_ShouldReturnCreatedTradeForBuyLot()
        {
            //arrange
            _lot.Type = Types.Buy;
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _userRepository.Setup(u => u.GetUserAsync(_user2.Id)).ReturnsAsync(_user2);
            _lotRepository.Setup(l => l.GetLotAsync(_lot.Id)).ReturnsAsync(_lot);
            TradeDTO tradeDTO = new TradeDTO
            {
                LotId = _lot.Id
            };
            Trade trade = new Trade
            {
                Buyer = _user2,
                TradeLot = _lot,
                LotId = tradeDTO.LotId,
                BuyerId = _user2.Id,
                TradeDate = DateTime.Now,
            };
            _tradeRepository.Setup(t => t.CreateTradeAsync(It.IsAny<Trade>())).ReturnsAsync(trade);
            _lotRepository.Setup(l => l.UpdateLotAsync(_lot.Id, _lot)).ReturnsAsync(_lot);
            //act
            var result = await _tradeService.CreateTrade(tradeDTO, _user2.Id);
            //assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateTrade_ShouldReturnCustomErrorBalanceNotEnoughForSoldLot()
        {
            //arrange
            var balance = _user.Balance.Where(b=>b.Currency=="USD").FirstOrDefault();
            balance.Amount = 0;
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _userRepository.Setup(u => u.GetUserAsync(_user2.Id)).ReturnsAsync(_user2);
            _lotRepository.Setup(l => l.GetLotAsync(_lot.Id)).ReturnsAsync(_lot);
            TradeDTO tradeDTO = new TradeDTO
            {
                LotId = _lot.Id
            };
            Trade trade = new Trade
            {
                Buyer = _user2,
                TradeLot = _lot,
                LotId = tradeDTO.LotId,
                BuyerId = _user2.Id,
                TradeDate = DateTime.Now,
            };
            _tradeRepository.Setup(t => t.CreateTradeAsync(It.IsAny<Trade>())).ReturnsAsync(trade);
            _lotRepository.Setup(l => l.UpdateLotAsync(_lot.Id, _lot)).ReturnsAsync(_lot);
            //act
            Assert.ThrowsAsync<NotEnoughBalanceForSold>(async()=>await _tradeService.CreateTrade(tradeDTO, _user2.Id));
        }

        [Fact]
        public async Task CreateTrade_ShouldReturnCustomErrorBalanceNotEnoughForBuyLot()
        {
            //arrange
            _lot.Type = Types.Buy;
            var balance = _user.Balance.Where(b => b.Currency == "RUB").FirstOrDefault();
            balance.Amount = 0;
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _userRepository.Setup(u => u.GetUserAsync(_user2.Id)).ReturnsAsync(_user2);
            _lotRepository.Setup(l => l.GetLotAsync(_lot.Id)).ReturnsAsync(_lot);
            TradeDTO tradeDTO = new TradeDTO
            {
                LotId = _lot.Id
            };
            Trade trade = new Trade
            {
                Buyer = _user2,
                TradeLot = _lot,
                LotId = tradeDTO.LotId,
                BuyerId = _user2.Id,
                TradeDate = DateTime.Now,
            };
            _tradeRepository.Setup(t => t.CreateTradeAsync(It.IsAny<Trade>())).ReturnsAsync(trade);
            _lotRepository.Setup(l => l.UpdateLotAsync(_lot.Id, _lot)).ReturnsAsync(_lot);
            //act
            Assert.ThrowsAsync<NotEnoughBalanceForBuy>(async () => await _tradeService.CreateTrade(tradeDTO, _user2.Id));
        }

        [Fact]
        public async Task GetTrade_ShouldReturnTrade()
        {
            //arrange
            Trade trade = new Trade
            {
                Id = 1,
                Buyer = _user2,
                TradeLot = _lot,
                LotId = _lot.Id,
                BuyerId = _user2.Id,
                TradeDate = DateTime.Now,
            };
            _tradeRepository.Setup(t=>t.GetTradeAsync(It.IsAny<int>())).ReturnsAsync(trade);
            //act
            var result = await _tradeService.GetTrade(trade.Id);
            //assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetTrades_ShouldReturnTrades()
        {
            //arrange
            List<Trade> trades = new List<Trade>{ new Trade
                {
                    Id = 1,
                    Buyer = _user2,
                    TradeLot = _lot,
                    LotId = _lot.Id,
                    BuyerId = _user2.Id,
                    TradeDate = DateTime.Now,
                }
            };
            _tradeRepository.Setup(t => t.GetTradesAsync()).ReturnsAsync(trades);
            //act
            var result = await _tradeService.GetTrades();
            //assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}
