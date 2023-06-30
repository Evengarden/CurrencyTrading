namespace CurrencyTrading.services.CustomExceptions
{
    public class OwnerIsBuyer : InvalidOperationException
    {
        public override string Message =>
           $"Cannot create trade. You can't buy your own lot";
    }
}
