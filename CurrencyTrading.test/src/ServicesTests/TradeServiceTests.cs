using CurrencyTrading.Data;
using CurrencyTrading.Helper;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
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
            _lot = _ctx.Lots.Add(new Lot
            {
                Currency = "USD",
                CurrencyAmount = 10,
                Status = 0,
                Price = 100,
                OwnerId = _user.Id
            }).Entity;
            _tradeRepository = new Mock<ITradeRepository>();
            _lotRepository = new Mock<ILotRepository>();
            _userRepository = new Mock<IUserRepository>();
            _tradeService = new TradeService(_tradeRepository.Object,_userRepository.Object,_lotRepository.Object);
        }

        [Fact]
        public async Task CreateTrade_ShouldReturnCreatedTrade()
        {

        }

        [Fact]
        public async Task GetTrade_ShouldReturnTrade()
        {

        }

        [Fact]
        public async Task GetTrades_ShouldReturnTrades()
        {

        }
    }
}
