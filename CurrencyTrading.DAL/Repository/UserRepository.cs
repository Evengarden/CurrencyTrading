using CurrencyTrading.Data;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using Microsoft.EntityFrameworkCore;

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
            currentUser = user;
            await _ctx.SaveChangesAsync();
            return currentUser;
        } 
        public async Task<User> GetUserAsync(int userId)
        {
            var user = await _ctx.Users.Include(u=>u.Balance).
                Include(u=>u.Lots).
                Include(u=>u.Trades).FirstOrDefaultAsync(u=>u.Id == userId);
            return user;
        }
        public async Task<User> GetUserByLogin(string login)
        {
            var user = await _ctx.Users.FirstOrDefaultAsync(u => u.Login == login);
            return user;
        }
    }
}
