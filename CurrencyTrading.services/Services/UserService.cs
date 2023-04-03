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

namespace CurrencyTrading.services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JWTSettings _config;
        public UserService(IUserRepository userRepository, IOptions<JWTSettings> config)
        {
            _userRepository = userRepository;
            _config = config.Value;

        }
        public async Task<string?> Auth(User user)
        {
            var findedUser = await _userRepository.CheckCredentails(user.Login, user.Password);
            if (findedUser != null)
            {
                var token = AuthHelper.GenerateJwtToken(findedUser, _config);
                return token;
            }

            return null;
        }

        public async Task<User> GetCurrentUser(int userId)
        {
           var user = await _userRepository.GetUserAsync(userId);
            return user;
        }

        public async Task UserRegistration(User user)
        {
            user.Password = HashPassword.HashPass(user.Password);
            await _userRepository.CreateUserAsync(user);
        }
    }
}
