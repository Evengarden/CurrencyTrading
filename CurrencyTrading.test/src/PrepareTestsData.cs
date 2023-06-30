using CurrencyTrading.Data;
using CurrencyTrading.Models;
using Microsoft.EntityFrameworkCore;
using System.Web.Helpers;

namespace CurrencyTrading.test.src
{
    internal class PrepareTestsData
    {
        internal static void InitDbCtx(out DataContext ctx)
        {
            var dbOptions = new DbContextOptionsBuilder<DataContext>()
                  .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                  .Options;
            ctx = new DataContext(dbOptions);
            ctx.Database.EnsureCreated();
            
        }
        internal static void InitUserInDb(DataContext ctx, out User user)
        {
            user = ctx.Users.Add(new User
            {
                Login = "test",
                Password = Crypto.HashPassword("test")
            }).Entity;
        }

        internal static void InitUserBalance(User user)
        {
            user.Balance = new List<Balance> {
                new Balance {
                Amount = 100,
                Currency ="USD",
                User = user
                },
                new Balance {
                Amount = 10000,
                Currency ="RUB",
                User = user
                }
            };
        }
    }
}
