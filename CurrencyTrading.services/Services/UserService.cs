using CurrencyTrading.Helper;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.services.Helpers;
using CurrencyTrading.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyTrading.DAL.DTO;
using CurrencyTrading.services.CustomExceptions;

namespace CurrencyTrading.services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBalanceRepository _balanceRepository;
        private readonly JWTSettings _config;
        public UserService(IUserRepository userRepository,IBalanceRepository balanceRepository, IOptions<JWTSettings> config)
        {
            _userRepository = userRepository;
            _balanceRepository = balanceRepository;
            _config = config.Value;

        }
        public async Task<string?> Auth(UserDTO user)
        {
            try
            {
                var findedUser = await _userRepository.CheckCredentails(user.Login, user.Password);
                if (findedUser != null)
                {
                    var token = AuthHelper.GenerateJwtToken(findedUser, _config);
                    return token;
                }

                return null;
            }
            catch (NullReferenceException)
            {
                throw new UserNotFound();
            }
        }

        public async Task<User> GetCurrentUser(int userId)
        {
           var user = await _userRepository.GetUserAsync(userId);
            return user;
        }

        public async Task<User> UpdateUser(int userId,UserDTO user)
        {
            var currentUser = await _userRepository.GetUserAsync(userId);
            if (currentUser.Login != user.Login)
            {
                currentUser.Login = user.Login;
            }
            if (currentUser.Password != user.Password)
            {
                currentUser.Password = user.Password;
            }
            return await _userRepository.UpdateUserAsync(userId, currentUser);
        }

        public async Task UserRegistration(User user)
        {
            user.Password = HashPassword.HashPass(user.Password);
            await _userRepository.CreateUserAsync(user);
            Balance balance = new Balance
            { 
                Currency = "RUB",
                Amount = 10000,
                User = user 
            };
            await _balanceRepository.CreateBalanceAsync(balance);
        }
    }
}
