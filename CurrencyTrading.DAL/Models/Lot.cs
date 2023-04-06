using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CurrencyTrading.Models
{
    public enum Statuses
    {
        Created,
        Solded
    }
    public enum Types
    {
        Sold,
        Buy
    }
    public class Lot
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Currency { get; set; }
        public decimal CurrencyAmount { get; set; }
        public decimal Price { get; set; }
        public int OwnerId { get; set; }
        public Types Type { get; set; }
        public Statuses Status { get; set; }
        public Trade? Trade { get; set; }
        [JsonIgnore]
        public User Owner { get; set; }
    }
}
