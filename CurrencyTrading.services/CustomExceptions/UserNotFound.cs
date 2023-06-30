namespace CurrencyTrading.services.CustomExceptions
{
    public class UserNotFound : NullReferenceException
    {
        public override string Message =>
           $"User not found";
    }
}
