using CurrencyTrading.Data;
using CurrencyTrading.Helper;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.Repository;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CurrencyTrading.test.src.RepositoryTests
{
    public class UserRepositoryTest : IDisposable
    {
        private readonly UserRepository _userRepository;
        private readonly DataContext _ctx;
        public UserRepositoryTest()
        {
            var dbOptions = new DbContextOptionsBuilder<DataContext>()
                     .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                     .Options;
            _ctx = new DataContext(dbOptions);
            _ctx.Database.EnsureCreated();
            _userRepository = new UserRepository(_ctx);

        }
        [Fact]
        public async Task UserRepository_ShouldReturnCreatedUserFromDb()
        {
            //arrange
            var user = prepareUserData("test1", "test1pas");
            //act
            var createdUser = await _userRepository.CreateUserAsync(user);
            //assert    
            Assert.NotNull(createdUser);
            Assert.Equal(user, createdUser);

            Dispose();
        }

        public async Task UserRepository_ShouldReturnErrorOfCreatedUserWithRequiredNullField()
        {
            //arrange
            var user = prepareUserData("test1", "test1pas");
            user.Login = null;
            //assert
            await Assert.ThrowsAsync<DbUpdateException>(async () => await _userRepository.CreateUserAsync(user));

            Dispose();
        }

        [Fact]
        public async Task UserRepository_ShouldReturnUpdatedUserFromDb()
        {
            //arrange
            string previousLogin = HashPassword.HashPass("testPassword");
            var user = prepareUserData("user1login", previousLogin);
            user.Login = "user2login";
            var createdUser = await _userRepository.CreateUserAsync(user);
            //act
            var updatedUser = await _userRepository.UpdateUserAsync(createdUser.Id, user);
            //assert
            Assert.NotNull(updatedUser);
            Assert.NotEqual(updatedUser.Login, previousLogin);

            Dispose();
        }

        [Fact]
        public async Task UserRepository_ShouldReturnNotEmptyEntityUserFromDb()
        {
            //arrange
            var notExistingUserId = -1;
            //act
            var foundedUser = await _userRepository.GetUserAsync(notExistingUserId);
            //assert    
            Assert.Null(foundedUser);

            Dispose();
        }

        [Fact]
        public async Task UserRepository_ShouldNotReturnNotExistedUserFromDb()
        {
            //arrange
            var user = prepareUserData("test3", "test3pas");
            var createdUser = await _userRepository.CreateUserAsync(user);
            //act
            var foundedUser = await _userRepository.GetUserAsync(createdUser.Id);
            //assert    
            Assert.NotNull(foundedUser);
            Assert.NotNull(foundedUser.Login);
            Assert.NotNull(foundedUser.Password);

            Dispose();
        }

        public User prepareUserData(string login, string password)
        {
            User user = new User
            {
                Login = login,
                Password = HashPassword.HashPass(password)
            };
            return user;
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }

    }
}