namespace CurrencyTrading.services.CustomExceptions
{
    public class TradeNotFound : NullReferenceException
    {
        public override string Message =>
         $"Trade not found";
    }
}
