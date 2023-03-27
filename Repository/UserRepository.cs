using CurrencyTrading.Data;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;

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
        public async  Task<User> CreateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        } 
        public async Task<User> GetUserAsync(int userId)
        {
            throw new NotImplementedException();
        }
        
        public async Task<bool> SaveAsync()
        {
            var saved = await _ctx.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
