using CurrencyTrading.DAL.DTO;

namespace CurrencyTrading.services.Interfaces
{
    public interface ICurrencyService
    {
        Task<ICollection<CurrencyDTO>> GetCurrency();
        Task<decimal> CalculateLotPrice(string currency, decimal currencyAmount);
        Task<string?> CheckCurrencyExist(string currency);
    }
}
