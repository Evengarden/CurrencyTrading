using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Helpers;

namespace CurrencyTrading.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public ICollection<Trade>? Trades { get; set; }  
        public ICollection<Lot>? Lots { get; set; }
        public ICollection<Balance>? Balance { get; set; }

    }
}
