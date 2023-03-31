using CurrencyTrading.Data;
using CurrencyTrading.Helper;
using CurrencyTrading.Models;
using CurrencyTrading.Repository;
using Microsoft.EntityFrameworkCore;

namespace CurrencyTrading.test.src.RepositoryTests
{
    public class LotRepositoryTests : IDisposable
    {
        private readonly DataContext _ctx;
        private readonly LotRepository _lotRepository;
        private readonly User _user;
        public LotRepositoryTests()
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
            _lotRepository = new LotRepository(_ctx);
        }
        [Fact]
        public async Task LotRepostory_ShouldReturnCreatedLotFromDb()
        {
            //arrange
            var lot = prepareLotData();
            //act
            var createdLot = await _lotRepository.CreateLotAsync(lot);
            //assert
            Assert.NotNull(createdLot);
            Assert.Equal("USD", createdLot.Currency);

            Dispose();
        }

        [Fact]
        public async Task LotRepostory_ShouldReturnErrorOfCreatedLotWithRequiredNullField()
        {
            //arrange
            var lot = prepareLotData();
            lot.Currency = null;
            //assert
            await Assert.ThrowsAsync<DbUpdateException>(async () => await _lotRepository.CreateLotAsync(lot));

            Dispose();
        }

        [Fact]
        public async Task LotRepository_ShouldReturnUpdatedLotFromDb()
        {
            //arrange
            var lot = prepareLotData();
            decimal previousPrice = lot.Price;
            var createdLot = await _lotRepository.CreateLotAsync(lot);
            createdLot.Price = 4;
            //act
            var updatedLot = await _lotRepository.UpdateLotAsync(createdLot.Id, createdLot);
            //assert
            Assert.NotNull(updatedLot);
            Assert.NotEqual(previousPrice, updatedLot.Price);
            Dispose();
        }

        [Fact]
        public async Task LotRepository_ShouldDeleteLotFromDb()
        {
            //arrange
            var lot = prepareLotData();
            var createdLot = await _lotRepository.CreateLotAsync(lot);
            //act
            var deletedLot = await _lotRepository.DeleteLotAsync(createdLot.Id);
            var findDeletedLot = await _ctx.Lots.FindAsync(deletedLot.Id); // ctx or repo method?
            //assert
            Assert.NotNull(deletedLot);
            Assert.Null(findDeletedLot);

            Dispose();
        }

        [Fact]
        public async Task LotRepository_ShouldNotDeleteNonExistingLotFromDb()
        {
            //arrange
            var nonExistingLotId = -1;
            //assert
            await Assert.ThrowsAsync<ArgumentNullException>
                (async () => await _lotRepository.DeleteLotAsync(nonExistingLotId));

            Dispose();
        }

        [Fact]
        public async Task LotRepository_ShouldReturnLotFromDb()
        {
            //arrange
            var lot = prepareLotData();
            var createdLot = await _lotRepository.CreateLotAsync(lot);
            //act
            var foundedLot = await _lotRepository.GetLotAsync(createdLot.Id);
            //assert
            Assert.NotNull(foundedLot);

            Dispose();
        }

        [Fact]
        public async Task LotRepository_ShouldNotReturnLotFromDb()
        {
            //arrange
            var nonExistId = -1;
            //act
            var foundedLot = await _lotRepository.GetLotAsync(nonExistId);
            //assert
            Assert.Null(foundedLot);

            Dispose();
        }
        [Fact]
        public async Task LotRepository_ShouldReturnAllLotsFromDb()
        {
            //arrange
            var lot = prepareLotData();
            var createdLot = await _lotRepository.CreateLotAsync(lot);
            //act
            var foundedLots = await _lotRepository.GetLotsAsync();
            //assert
            Assert.NotNull(foundedLots);
            Assert.IsAssignableFrom<ICollection<Lot>>(foundedLots);

            Dispose();
        }
        public Lot prepareLotData()
        {
            Lot lot = new Lot
            {
                Currency = "USD",
                CurrencyAmount = 10,
                Status = 0,
                Price = 100,
                OwnerId = _user.Id
            };
            return lot;
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }
    }
}
