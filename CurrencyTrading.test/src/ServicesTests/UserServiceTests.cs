using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Data;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.Repository;
using CurrencyTrading.services.CustomExceptions;
using CurrencyTrading.services.Helpers;
using CurrencyTrading.services.Interfaces;
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
        private readonly Mock<IAuthService> _authService;

        public UserServiceTests()
        {
            PrepareTestsData.InitDbCtx(out _ctx);
            PrepareTestsData.InitUserInDb(_ctx, out _user);
            _userRepository = new Mock<IUserRepository>();
            _balanceRepository = new Mock<IBalanceRepository>();
            _authService = new Mock<IAuthService>();
            _userService = new UserService(_userRepository.Object,_balanceRepository.Object, _authService.Object);
        }

        [Fact]
        public async Task Auth_ShouldReturnJWTToken()
        {
            //arrange
            _userRepository.Setup(u => u.GetUserByLogin(_user.Login)).ReturnsAsync(_user);
            UserDTO user = new UserDTO
            {
                Login = _user.Login,
                Password = _user.Password
            };
            _authService.Setup(u => u.VerifyPass(user.Password,_user.Password)).Returns(true);
            _authService.Setup(u => u.GenerateJwtToken(_user)).Returns("token");
            //act
            var currentUser = await _userService.Auth(user);
            //assert
            Assert.NotNull(currentUser);
        }

        [Fact]
        public async Task Auth_ShouldNotReturnJWTToken()
        {
            //arrange
            _userRepository.Setup(u => u.GetUserByLogin(_user.Login)).ReturnsAsync(_user);
            UserDTO user = new UserDTO
            {
                Login = _user.Login,
                Password = _user.Password
            };
            _authService.Setup(u => u.VerifyPass(user.Password, _user.Password)).Returns(false);
            //act + assert
            await Assert.ThrowsAsync<UserNotFound>(async () => await _userService.Auth(user));
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
