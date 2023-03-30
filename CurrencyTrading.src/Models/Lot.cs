using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyTrading.Models
{
    public enum Statuses
    {
        Created,
        Solded
    }
    public class Lot
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Currency { get; set; }
        public decimal CurrencyAmount { get; set; }
        public decimal Price { get; set; }
        public Statuses Status { get; set; }
        public Trade? Trade { get; set; }
        public User Owner { get; set; }
    }
}
