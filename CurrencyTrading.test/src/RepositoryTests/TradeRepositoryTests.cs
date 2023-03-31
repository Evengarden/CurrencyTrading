using CurrencyTrading.Data;
using CurrencyTrading.Helper;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.Repository;
using Microsoft.EntityFrameworkCore;

namespace CurrencyTrading.test.src.RepositoryTests
{
    public class TradeRepositoryTests : IDisposable
    {
        private readonly DataContext _ctx;
        private readonly TradeRepository _tradeRepository;
        private readonly User _buyer;
        private readonly Lot _lot;

        public TradeRepositoryTests()
        {
            var dbOptions = new DbContextOptionsBuilder<DataContext>()
                   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                   .Options;
            _ctx = new DataContext(dbOptions);
            _ctx.Database.EnsureCreated();
            var user1 = _ctx.Users.Add(new User
            {
                Login = "test",
                Password = HashPassword.HashPass("test")
            });
            _buyer = _ctx.Users.Add(new User
            {
                Login = "test2",
                Password = HashPassword.HashPass("test2")
            }).Entity;
            _lot = _ctx.Lots.Add(new Lot
            {
                Currency = "USD",
                CurrencyAmount = 10,
                Status = 0,
                Price = 100,
                OwnerId = user1.Entity.Id
            }).Entity;
            _tradeRepository = new TradeRepository(_ctx);
        }

        [Fact]
        public async Task TradeRepository_ShouldReturnCreatedTradeFromDb()
        {
            //arrange
            var trade = prepareTradeData();
            //act
            var createdTrade = await _tradeRepository.CreateTradeAsync(trade);
            //assert
            Assert.NotNull(createdTrade);
            Assert.Equal(_buyer.Id, createdTrade.Buyer.Id);

            Dispose();
        }

        [Fact]
        public async Task TradeRepository_ShouldReturnFoundedTradeFromDb()
        {
            //arrange
            var trade = prepareTradeData();
            var createdTrade = await _tradeRepository.CreateTradeAsync(trade);
            //act
            var foundedTrade = await _tradeRepository.GetTradeAsync(createdTrade.Id);
            //assert
            Assert.NotNull(foundedTrade);

            Dispose();
        }

        [Fact]
        public async Task TradeRepository_ShouldNotReturnFoundedTradeFromDb()
        {
            //arrange
            var nonExistId = -1;
            //act
            var foundedTrade = await _tradeRepository.GetTradeAsync(nonExistId);
            //assert
            Assert.Null(foundedTrade);

            Dispose();
        }

        [Fact]
        public async Task TradeRepository_ShouldReturnAllTradesFromDb()
        {
            //arrange
            var trade = prepareTradeData();
            var createdTrade = await _tradeRepository.CreateTradeAsync(trade);
            //act
            var foundedTrades = await _tradeRepository.GetTradesAsync();
            //assert
            Assert.NotNull(foundedTrades);
            Assert.IsAssignableFrom<ICollection<Trade>>(foundedTrades);

            Dispose();
        }


        public Trade prepareTradeData()
        {
            Trade trade = new Trade
            {
                TradeDate = DateTime.Now,
                BuyerId = _buyer.Id,
                LotId = _lot.Id
            };
            return trade;
        }
        public void Dispose()
        {
            _ctx.Dispose();
        }
    }
}
