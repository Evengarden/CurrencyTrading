using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyTrading.Models
{
    public class Balance
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
