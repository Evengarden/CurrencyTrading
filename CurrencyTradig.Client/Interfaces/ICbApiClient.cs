using CurrencyTrading.DAL.DTO;

namespace CurrencyTrading.Client.Interfaces
{
    public interface ICbApiClient
    {
        Task<ICollection<CurrencyDTO>> sendRequestToCb();
    }
}
