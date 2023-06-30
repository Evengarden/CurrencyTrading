using System.Security.Claims;
using CurrencyTrading.Controllers;
using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Data;
using CurrencyTrading.Models;
using CurrencyTrading.services.CustomExceptions;
using CurrencyTrading.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;

namespace CurrencyTrading.test.src.ControllerTests
{
    public class TradeControllerTests
	{
        private readonly TradeController _tradeController;
        private readonly Mock<ITradeService> _tradeService;
        private readonly Mock<IAuthService> _authService;
        private readonly DataContext _ctx;
        private readonly User _user;
        private readonly User _user2;
        private readonly Lot _lot;
        public TradeControllerTests()
		{
            PrepareTestsData.InitDbCtx(out _ctx);
            PrepareTestsData.InitUserInDb(_ctx,out _user);
            PrepareTestsData.InitUserInDb(_ctx,out _user2);
            _lot = _ctx.Lots.Add(new Lot
            {
                Id = 1,
                Status = Statuses.Created,
                Automatch = Automatch.Off,
                Currency = "USD",
                CurrencyAmount = 10,
                Owner = _user,
                Price = 1000,
                Type = Types.Sold,
                OwnerId = _user.Id
            }).Entity;
            var claims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                    new Claim("ID", _user.Id.ToString()),
                }, "mock"));
            _tradeService = new Mock<ITradeService>();
            _authService = new Mock<IAuthService>();
            _tradeController = new TradeController(_tradeService.Object,_authService.Object);
            _tradeController.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claims }
            };
        }

        [Fact]
        public async Task GetTrades_ShouldReturnIActionResultWithTrades()
        {
            //arrange
            List<Trade> trades = new List<Trade>
            {
                new Trade
                {
                    Buyer = _user2,
                    BuyerId = _user2.Id,
                    LotId=_lot.Id,
                    TradeDate=DateTime.Now,
                    TradeLot=_lot
                }
            };
            _tradeService.Setup(t=>t.GetTrades()).ReturnsAsync(trades);
            //act
            var result = await _tradeController.GetTrades();
            //assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetTrade_ShouldReturnIActionResultWithTrade()
        {
            //arrange
            Trade trade = new Trade
            {
                Buyer = _user2,
                BuyerId = _user2.Id,
                LotId = _lot.Id,
                TradeDate = DateTime.Now,
                TradeLot = _lot
            };
            _tradeService.Setup(t => t.GetTrade(trade.Id)).ReturnsAsync(trade);
            //act
            var result = await _tradeController.GetTrade(trade.Id);
            //assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task CreateTrade_ShouldReturnIActionResultWithCreatedTrade()
        {
            //arrange
            TradeDTO tradeDTO = new TradeDTO
            {
                LotId=_lot.Id
            };
            Trade trade = new Trade
            {
                Buyer = _user2,
                BuyerId = _user2.Id,
                LotId = tradeDTO.LotId,
                TradeDate = DateTime.Now,
                TradeLot = _lot
            };
            _tradeService.Setup(t => t.CreateTrade(tradeDTO,_user2.Id)).ReturnsAsync(trade);
            //act
            var result = await _tradeController.CreateTrade(tradeDTO);
            //assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task CreateTrade_ShouldReturnCustomErrorNotEnoughBalanceForSold()
        {
            //arrange
            TradeDTO tradeDTO = new TradeDTO
            {
                LotId = _lot.Id
            };
            Trade trade = new Trade
            {
                Buyer = _user2,
                BuyerId = _user2.Id,
                LotId = tradeDTO.LotId,
                TradeDate = DateTime.Now,
                TradeLot = _lot
            };
            _tradeService.Setup(t => t.CreateTrade(tradeDTO, _user2.Id)).ThrowsAsync(new NotEnoughBalanceForSold());
            //act
            Assert.ThrowsAsync<NotEnoughBalanceForSold>(async () => await _tradeController.CreateTrade(tradeDTO));
        }
        [Fact]
        public async Task CreateTrade_ShouldReturnCustomErrorNotEnoughBalanceForBuy()
        {
            //arrange
            TradeDTO tradeDTO = new TradeDTO
            {
                LotId = _lot.Id
            };
            Trade trade = new Trade
            {
                Buyer = _user2,
                BuyerId = _user2.Id,
                LotId = tradeDTO.LotId,
                TradeDate = DateTime.Now,
                TradeLot = _lot
            };
            _tradeService.Setup(t => t.CreateTrade(tradeDTO, _user2.Id)).ThrowsAsync(new NotEnoughBalanceForBuy());
            //act
            Assert.ThrowsAsync<NotEnoughBalanceForBuy>(async () => await _tradeController.CreateTrade(tradeDTO));
        }
    }
}

