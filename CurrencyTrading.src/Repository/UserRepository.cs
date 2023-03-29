using CurrencyTrading.Data;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyTrading.Repository
{
    // TODO: Реализовать репозиторий для сущности User
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
            await SaveAsync();
            return addedUser.Entity;
        }

        public async Task<User> UpdateUserAsync(int userId,User user)
        {
            var currentUser = await _ctx.Users.FindAsync(userId);
            currentUser.Login = user.Login;
            currentUser.Password = user.Password;
            await SaveAsync();
            return currentUser;
        } 
        public async Task<User> GetUserAsync(int userId)
        {
            var user = await _ctx.Users.FindAsync(userId);
            return user;
        }
        
        public async Task<bool> SaveAsync()
        {
            var saved = await _ctx.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
