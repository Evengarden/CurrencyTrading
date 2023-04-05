using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyTrading.Models
{
    public class User
    {
        public User()
        {
            Balance = new HashSet<Balance>();
            Lots = new HashSet<Lot>();
            Trades = new HashSet<Trade>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public ICollection<Trade>? Trades { get; set; }  
        public ICollection<Lot>? Lots { get; set; }
        public ICollection<Balance>? Balance { get; set; }

    }
}
