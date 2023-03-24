namespace CurrencyTrading.Models
{
    public class Balance
    {
        public int Id { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
