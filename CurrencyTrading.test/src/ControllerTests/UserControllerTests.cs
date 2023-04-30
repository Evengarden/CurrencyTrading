﻿using System;
using System.Security.Claims;
using CurrencyTrading.Controllers;
using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Data;
using CurrencyTrading.Helper;
using CurrencyTrading.Models;
using CurrencyTrading.services.CustomExceptions;
using CurrencyTrading.services.Interfaces;
using CurrencyTrading.services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CurrencyTrading.test.src.ControllerTests
{
	public class UserControllerTests
	{
        private readonly UserController _userController;
        private readonly Mock<IUserService> _userService;
        private readonly DataContext _ctx;
        private readonly User _user;
        public UserControllerTests()
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
            var claims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                    new Claim("ID", _user.Id.ToString()),
                }, "mock"));
            _userService = new Mock<IUserService>();
            _userController = new UserController(_userService.Object);
            _userController.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claims }
            };
        }

        public async Task CreateUser_ShouldReturnIActionResultWithCreatedUser()
        {
            //arrange
            User user = new User
            {
                Login="test_user",
                Password="test_user"
            };
            //act
            var result = await _userController.Registration(user);
            //assert
            Assert.NotNull(result);
        }
        public async Task CreateUser_ShouldReturnDbUpdateException()
        {
            //arrange
            User user = new User
            {
                Login = "test_user",
                Password = "test_user"
            };
            _userService.Setup(u => u.UserRegistration(It.IsAny<User>())).ThrowsAsync(new DbUpdateException());
            //act
            Assert.ThrowsAsync<DbUpdateException>(async () =>await _userController.Registration(user));
        }
        public async Task Auth_ShouldReturnToken()
        {
            //arrange
            UserDTO user = new UserDTO
            {
                Login = "test_user",
                Password = "test_user"
            };
            _userService.Setup(u => u.Auth(It.IsAny<UserDTO>())).ReturnsAsync("test");
            //act
            var result = await _userController.Authorization(user);
            //assert
            Assert.NotNull(result);
        }
        public async Task Auth_ShouldReturnInvalidCredentailsError()
        {
            //arrange
            UserDTO user = new UserDTO
            {
                Login = "test_user",
                Password = "test_user"
            };
            _userService.Setup(u => u.Auth(It.IsAny<UserDTO>())).ThrowsAsync(new UserNotFound());
            //act
            Assert.ThrowsAsync<UserNotFound>(async () => await _userController.Authorization(user));
        }
        public async Task GetUser_ShouldReturnFindedUser()
        {
            //arrange
            _userService.Setup(u => u.GetCurrentUser(_user.Id)).ReturnsAsync(_user);
            //act
            var result = await _userController.GetUserAsync();
            //assert
            Assert.NotNull(result);
        }
        public async Task UpdateUser_ShouldReturnUpdatedUser()
        {
            //arrange
            UserDTO user = new UserDTO
            {
                Login = "test_user",
                Password = "test_user"
            };
            _userService.Setup(u => u.UpdateUser(_user.Id,user)).ReturnsAsync(_user);
            //act
            var result = await _userController.UpdateUserAsync(user);
            //assert
            Assert.NotNull(result);
        }
    }
}

