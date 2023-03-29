namespace CurrencyTrading.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public ICollection<Trade> Trades { get; set; }  
        public ICollection<Lot> Lots { get; set; }
        public ICollection<Balance> Balance { get; set; }

    }
}
