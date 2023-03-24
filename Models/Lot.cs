namespace CurrencyTrading.Models
{
    public class Lot
    {
        public int Id { get; set; }
        public string Currency { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public Trade Trade { get; set; }
        public User Owner { get; set; }
    }
}
