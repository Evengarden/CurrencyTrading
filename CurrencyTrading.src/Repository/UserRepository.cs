using CurrencyTrading.Data;
using CurrencyTrading.Helper;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace CurrencyTrading.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _ctx;

        public UserRepository(DataContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<User> CreateUserAsync(User user)
        {
            var addedUser  = await _ctx.AddAsync(user);
            await _ctx.SaveChangesAsync();
            return addedUser.Entity;
        }

        public async Task<User> UpdateUserAsync(int userId,User user)
        {
            var currentUser = await _ctx.Users.FindAsync(userId);
            if (currentUser.Login != user.Login)
            {
                currentUser.Login = user.Login;
            }
            if (currentUser.Password != user.Password)
            {
                currentUser.Password = user.Password;
            }
            await _ctx.SaveChangesAsync();
            return currentUser;
        } 
        public async Task<User> GetUserAsync(int userId)
        {
            var user = await _ctx.Users.FindAsync(userId);
            return user;
        }

        public async Task<User> Auth(string login,string password)
        {
            var user = await _ctx.Users.FirstOrDefaultAsync(u => u.Login == login);
            bool isAuth = HashPassword.VerifyPass(user.Password,password);
            if (isAuth)
            {
                return user;

            }
            return null;
        }
    }
}
