using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.services.Interfaces;
using CurrencyTrading.DAL.DTO;
using CurrencyTrading.services.CustomExceptions;
using CurrencyTrading.DAL.Helpers;

namespace CurrencyTrading.services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBalanceRepository _balanceRepository;
        private readonly IAuthService _authService;
        public UserService(IUserRepository userRepository,IBalanceRepository balanceRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _balanceRepository = balanceRepository;
            _authService = authService;

        }
        public async Task<string?> Auth(UserDTO user)
        {
            var findedUser = await _userRepository.GetUserByLogin(user.Login);
            if (findedUser != null)
            {
                bool isAuth = _authService.VerifyPass(findedUser.Password, user.Password);
                if (isAuth)
                {
                    var token = _authService.GenerateJwtToken(findedUser);
                    return token;
                }
            }
            throw new UserNotFound();
        }

        public async Task<User> GetCurrentUser(int userId)
        {
           var user = await _userRepository.GetUserAsync(userId);
            return user;
        }

        public async Task<User> UpdateUser(int userId,UserDTO user)
        {
            user.Password = _authService.HashPass(user.Password);
            var currentUser = await _userRepository.GetUserAsync(userId);
            var newUser = UpdateEntityHelper.updateEntity(user, currentUser);
            return await _userRepository.UpdateUserAsync(userId, newUser);
        }

        public async Task UserRegistration(User user)
        {
            user.Password = _authService.HashPass(user.Password);
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
