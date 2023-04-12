using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.services.CustomExceptions
{
    public class UserNotFound : NullReferenceException
    {
        public override string Message =>
           $"User not found";
    }
}
