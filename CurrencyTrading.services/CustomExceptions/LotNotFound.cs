namespace CurrencyTrading.services.CustomExceptions
{
    public class LotNotFound : NullReferenceException
    {
        public override string Message =>
         $"Lot not found";
    }
}
