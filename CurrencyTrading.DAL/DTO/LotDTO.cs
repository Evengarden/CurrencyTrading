using CurrencyTrading.Models;

namespace CurrencyTrading.DAL.DTO
{
    public class LotDTO
    {
        public string Currency { get; set; }
        public decimal CurrencyAmount { get; set; }
        public decimal Price { get; set; }
        public Types Type { get; set; }
        public Automatch Automatch { get; set; }
    }
}
