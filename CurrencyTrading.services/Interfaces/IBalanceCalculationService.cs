using CurrencyTrading.Models;

namespace CurrencyTrading.services.Interfaces
{
    public interface IBalanceCalculationService
    {
        public void CheckEnoughBalanceForSold(User user, Lot lot);
        public void CheckEnoughBalanceForBuy(User user, Lot lot);
        public Task<User> CalculateBalance(User buyer,User lotOwner, Lot lot);
    }
}
