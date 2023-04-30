using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Data;
using CurrencyTrading.Helper;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.Repository;
using CurrencyTrading.services.CustomExceptions;
using CurrencyTrading.services.Helpers;
using CurrencyTrading.services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.test.src.ServicesTests
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly DataContext _ctx;
        private readonly User _user;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IBalanceRepository> _balanceRepository;
        private readonly Mock<IOptions<JWTSettings>> _config;

        public UserServiceTests()
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
            _userRepository = new Mock<IUserRepository>();
            _balanceRepository = new Mock<IBalanceRepository>();
            _config = new Mock<IOptions<JWTSettings>>();
            _config.Setup(c => c.Value).Returns(new JWTSettings{
                Audience = "test",
                Issuer = "test",
                SecretKey = "jwt-secret-testtesttesttesttest",
            });
            _userService = new UserService(_userRepository.Object,_balanceRepository.Object,_config.Object);
        }

        [Fact]
        public async Task Auth_ShouldReturnJWTToken()
        {
            //arrange
            _userRepository.Setup(u => u.CheckCredentails(_user.Login, _user.Password)).ReturnsAsync(_user);
            UserDTO user = new UserDTO
            {
                Login = _user.Login,
                Password = _user.Password
            };
            //act
            var currentUser = await _userService.Auth(user);
            //assert
            Assert.NotNull(currentUser);
        }

        [Fact]
        public async Task Auth_ShouldNotReturnJWTToken()
        {
            //arrange
            _userRepository.Setup(u => u.CheckCredentails("", "")).ReturnsAsync(_user);
            UserDTO user = new UserDTO
            {
                Login = _user.Login,
                Password = _user.Password
            };
            //act
            var currentUser = await _userService.Auth(user);
            //assert
            Assert.Null(currentUser);
        }

        [Fact]
        public async Task GetCurrentUser_ShouldReturnTestUser()
        {
            //arrange
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            //act
            var currentUser = await _userService.GetCurrentUser(_user.Id);
            //assert
            Assert.NotNull(currentUser);
        }

        [Fact]
        public async Task GetCurrentUser_ShouldNotReturnTestUser()
        {
            //arrange
            _userRepository.Setup(u => u.GetUserAsync(-1)).ReturnsAsync(_user);
            //act
            var currentUser = await _userService.GetCurrentUser(_user.Id);
            //assert
            Assert.Null(currentUser);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnUpdatedTestUser()
        {
            //arrange
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _userRepository.Setup(u => u.UpdateUserAsync(_user.Id,_user)).ReturnsAsync(_user);
            UserDTO user = new UserDTO
            {
                Login = "new login",
                Password = "new password"
            };
            //act
            var updatedUser = await _userService.UpdateUser(_user.Id,user);
            //assert
            Assert.Equal(updatedUser.Login,user.Login);
            Assert.Equal(updatedUser.Password,user.Password);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnNull()
        {
            //arrange
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _userRepository.Setup(u => u.UpdateUserAsync(-1, _user)).ReturnsAsync(_user);
            UserDTO user = new UserDTO
            {
                Login = "new login",
                Password = "new password"
            };
            //act
            var updatedUser = await _userService.UpdateUser(_user.Id, user);
            //assert
            Assert.Null(updatedUser);
        }

        [Fact]
        public async Task UserRegistration_ShouldReturnNewUser()
        {
            //arrange
            User newUser = new User
            {
                Login = "new user",
                Password = "new password",

            };
            _balanceRepository.Setup(b => b.CreateBalanceAsync(It.IsAny<Balance>())).
                Callback<Balance>(c => newUser.Balance = new Balance[] { c }).ReturnsAsync((Balance balance) => balance);
            //act
            await _userService.UserRegistration(newUser);
            //assert
            Assert.NotNull(newUser);
            Assert.NotEmpty(newUser.Balance);
        }
    }
}
