namespace CurrencyTrading.Models
{
    public class Balance
    {
        public int Id { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public User User { get; set; }
    }
}
