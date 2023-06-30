using CurrencyTrading.Data;
using CurrencyTrading.Models;
using CurrencyTrading.Repository;

namespace CurrencyTrading.test.src.RepositoryTests
{
    public class TradeRepositoryTests
    {
        private readonly DataContext _ctx;
        private readonly TradeRepository _tradeRepository;
        private readonly User _buyer;
        private readonly User _owner;
        private readonly Lot _lot;

        public TradeRepositoryTests()
        {
            PrepareTestsData.InitDbCtx(out _ctx);
            PrepareTestsData.InitUserInDb(_ctx,out _buyer);

            PrepareTestsData.InitUserInDb(_ctx, out _owner);

            _lot = _ctx.Lots.Add(new Lot
            {
                Currency = "USD",
                CurrencyAmount = 10,
                Status = 0,
                Price = 100,
                OwnerId = _owner.Id
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
    }
}
