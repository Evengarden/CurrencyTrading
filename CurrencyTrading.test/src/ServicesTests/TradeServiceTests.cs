using AutoMapper;
using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Data;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.services.CustomExceptions;
using CurrencyTrading.services.Interfaces;
using CurrencyTrading.services.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CurrencyTrading.test.src.ServicesTests
{
    public class TradeServiceTests
    {
        private readonly TradeService _tradeService;
        private readonly DataContext _ctx;
        private readonly User _owner;
        private readonly User _buyer;
        private readonly Lot _lot;
        private readonly Mock<IUserRepository> _ownerRepository;
        private readonly Mock<ITradeRepository> _tradeRepository;
        private readonly Mock<ILotRepository> _lotRepository;
        private readonly Mock<IBalanceCalculationService> _balanceCalculationService;

        public TradeServiceTests()
        {
            PrepareTestsData.InitDbCtx(out _ctx);
            PrepareTestsData.InitUserInDb(_ctx,out _owner);
            PrepareTestsData.InitUserInDb(_ctx,out _buyer);
            PrepareTestsData.InitUserBalance(_owner);
            PrepareTestsData.InitUserBalance(_buyer);
            _lot = _ctx.Lots.Add(new Lot
            {
                Currency = "USD",
                CurrencyAmount = 10,
                Status = Statuses.Created,
                Price = 100,
                OwnerId = _owner.Id,
                Type = Types.Sold
            }).Entity;
            _tradeRepository = new Mock<ITradeRepository>();
            _lotRepository = new Mock<ILotRepository>();
            _ownerRepository = new Mock<IUserRepository>();
            _balanceCalculationService = new Mock<IBalanceCalculationService>();
            _tradeService = new TradeService(_tradeRepository.Object,_ownerRepository.Object,_lotRepository.Object, _balanceCalculationService.Object);
        }

        [Fact]
        public async Task CreateTrade_ShouldReturnCreatedTradeForSoldLot()
        {
            //arrange
            _ownerRepository.Setup(u => u.GetUserAsync(_owner.Id)).ReturnsAsync(_owner);
            _ownerRepository.Setup(u => u.GetUserAsync(_buyer.Id)).ReturnsAsync(_buyer);
            _lotRepository.Setup(l=>l.GetLotAsync(_lot.Id)).ReturnsAsync(_lot);
            TradeDTO tradeDTO = new TradeDTO
            {
                LotId=_lot.Id
            };
            Trade trade = new Trade
            {
                Buyer = _buyer,
                TradeLot=_lot,
                LotId = tradeDTO.LotId,
                BuyerId = _buyer.Id,
                TradeDate = DateTime.Now,
            };
            _tradeRepository.Setup(t => t.CreateTradeAsync(It.IsAny<Trade>())).ReturnsAsync(trade);
            _lotRepository.Setup(l => l.UpdateLotAsync(_lot.Id,_lot)).ReturnsAsync(_lot);
            //act
            var result = await _tradeService.CreateTrade(tradeDTO, _buyer.Id);
            //assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateTrade_ShouldReturnNotEnoughBalanceForBuy()
        {
            //arrange
            _ownerRepository.Setup(u => u.GetUserAsync(_owner.Id)).ReturnsAsync(_owner);
            _ownerRepository.Setup(u => u.GetUserAsync(_buyer.Id)).ReturnsAsync(_buyer);
            _lotRepository.Setup(l => l.GetLotAsync(_lot.Id)).ReturnsAsync(_lot);
            TradeDTO tradeDTO = new TradeDTO
            {
                LotId = _lot.Id
            };
            Trade trade = new Trade
            {
                Buyer = _buyer,
                TradeLot = _lot,
                LotId = tradeDTO.LotId,
                BuyerId = _buyer.Id,
                TradeDate = DateTime.Now,
            };
            _tradeRepository.Setup(t => t.CreateTradeAsync(It.IsAny<Trade>())).ReturnsAsync(trade);
            _lotRepository.Setup(l => l.UpdateLotAsync(_lot.Id, _lot)).ReturnsAsync(_lot);
            _balanceCalculationService.Setup(b => b.CheckEnoughBalanceForSold(_buyer,_lot)).Throws<NotEnoughBalanceForBuy>();
            //act + assert
            Assert.ThrowsAsync<NotEnoughBalanceForBuy>(async () => await _tradeService.CreateTrade(tradeDTO, _buyer.Id));
        }

        [Fact]
        public async Task CreateTrade_ShouldReturnCreatedTradeForBuyLot()
        {
            //arrange
            _lot.Type = Types.Buy;
            _ownerRepository.Setup(u => u.GetUserAsync(_owner.Id)).ReturnsAsync(_owner);
            _ownerRepository.Setup(u => u.GetUserAsync(_buyer.Id)).ReturnsAsync(_buyer);
            _lotRepository.Setup(l => l.GetLotAsync(_lot.Id)).ReturnsAsync(_lot);
            TradeDTO tradeDTO = new TradeDTO
            {
                LotId = _lot.Id
            };
            Trade trade = new Trade
            {
                Buyer = _buyer,
                TradeLot = _lot,
                LotId = tradeDTO.LotId,
                BuyerId = _buyer.Id,
                TradeDate = DateTime.Now,
            };
            _tradeRepository.Setup(t => t.CreateTradeAsync(It.IsAny<Trade>())).ReturnsAsync(trade);
            _lotRepository.Setup(l => l.UpdateLotAsync(_lot.Id, _lot)).ReturnsAsync(_lot);
            //act
            var result = await _tradeService.CreateTrade(tradeDTO, _buyer.Id);
            //assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateTrade_ShouldReturnCustomErrorBalanceNotEnoughForSoldLot()
        {
            //arrange
            var balance = _owner.Balance.Where(b=>b.Currency=="USD").FirstOrDefault();
            balance.Amount = 0;
            _ownerRepository.Setup(u => u.GetUserAsync(_owner.Id)).ReturnsAsync(_owner);
            _ownerRepository.Setup(u => u.GetUserAsync(_buyer.Id)).ReturnsAsync(_buyer);
            _lotRepository.Setup(l => l.GetLotAsync(_lot.Id)).ReturnsAsync(_lot);
            TradeDTO tradeDTO = new TradeDTO
            {
                LotId = _lot.Id
            };
            Trade trade = new Trade
            {
                Buyer = _buyer,
                TradeLot = _lot,
                LotId = tradeDTO.LotId,
                BuyerId = _buyer.Id,
                TradeDate = DateTime.Now,
            };
            _tradeRepository.Setup(t => t.CreateTradeAsync(It.IsAny<Trade>())).ReturnsAsync(trade);
            _lotRepository.Setup(l => l.UpdateLotAsync(_lot.Id, _lot)).ReturnsAsync(_lot);
            _balanceCalculationService.Setup(b => b.CheckEnoughBalanceForSold(_buyer, _lot)).Throws<NotEnoughBalanceForSold>();

            //act + assert
            Assert.ThrowsAsync<NotEnoughBalanceForSold>(async()=>await _tradeService.CreateTrade(tradeDTO, _buyer.Id));
        }

        [Fact]
        public async Task CreateTrade_ShouldReturnCustomErrorLotNotFound()
        {
            //arrange
            var balance = _owner.Balance.Where(b => b.Currency == "USD").FirstOrDefault();
            balance.Amount = 0;
            _ownerRepository.Setup(u => u.GetUserAsync(_owner.Id)).ReturnsAsync(_owner);
            _ownerRepository.Setup(u => u.GetUserAsync(_buyer.Id)).ReturnsAsync(_buyer);
            _lotRepository.Setup(l => l.GetLotAsync(_lot.Id)).ReturnsAsync(_lot);
            TradeDTO tradeDTO = new TradeDTO
            {
                LotId = -123
            };
            Trade trade = new Trade
            {
                Buyer = _buyer,
                TradeLot = _lot,
                LotId = tradeDTO.LotId,
                BuyerId = _buyer.Id,
                TradeDate = DateTime.Now,
            };
            _tradeRepository.Setup(t => t.CreateTradeAsync(It.IsAny<Trade>())).ReturnsAsync(trade);

            //act + assert
            Assert.ThrowsAsync<LotNotFound>(async () => await _tradeService.CreateTrade(tradeDTO, _buyer.Id));
        }

        [Fact]
        public async Task GetTrade_ShouldReturnTrade()
        {
            //arrange
            Trade trade = new Trade
            {
                Id = 1,
                Buyer = _buyer,
                TradeLot = _lot,
                LotId = _lot.Id,
                BuyerId = _buyer.Id,
                TradeDate = DateTime.Now,
            };
            _tradeRepository.Setup(t=>t.GetTradeAsync(It.IsAny<int>())).ReturnsAsync(trade);
            //act
            var result = await _tradeService.GetTrade(trade.Id);
            //assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetTrades_ShouldReturnNotNullTradesCollection()
        {
            //arrange
            List<Trade> trades = new List<Trade>{ new Trade
                {
                    Id = 1,
                    Buyer = _buyer,
                    TradeLot = _lot,
                    LotId = _lot.Id,
                    BuyerId = _buyer.Id,
                    TradeDate = DateTime.Now,
                }
            };
            _tradeRepository.Setup(t => t.GetTradesAsync()).ReturnsAsync(trades);
            //act
            var result = await _tradeService.GetTrades();
            //assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetTrades_ShouldReturnNotEmptyTradesCollection()
        {
            //arrange
            List<Trade> trades = new List<Trade>{ new Trade
                {
                    Id = 1,
                    Buyer = _buyer,
                    TradeLot = _lot,
                    LotId = _lot.Id,
                    BuyerId = _buyer.Id,
                    TradeDate = DateTime.Now,
                }
            };
            _tradeRepository.Setup(t => t.GetTradesAsync()).ReturnsAsync(trades);
            //act
            var result = await _tradeService.GetTrades();
            //assert
            Assert.NotEmpty(result);
        }
    }
}
