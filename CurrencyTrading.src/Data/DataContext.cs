using CurrencyTrading.Helper;
using CurrencyTrading.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace CurrencyTrading.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<Balance> Balances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasIndex(u => u.Login).IsUnique();
            modelBuilder.Entity<User>().HasMany(u => u.Balance).WithOne(b=>b.User);
            User user1 = new User
            {
                Id = 1,
                Login = "test1",
               Password = HashPassword.HashPass("test1"),
            };
            User user2 = new User
            {
                Id = 2,
                Login = "test2",
                Password = HashPassword.HashPass("test2"),
            };
            modelBuilder.Entity<User>().HasData(user1);
            modelBuilder.Entity<User>().HasData(user2);

            modelBuilder.Entity<Trade>().HasKey(t => t.Id);
            modelBuilder.Entity<Trade>().HasOne(t => t.TradeLot).WithOne(l => l.Trade);
            modelBuilder.Entity<Trade>().HasOne(t => t.Buyer).WithMany(u=> u.Trades);

            modelBuilder.Entity<Lot>().HasKey(l => l.Id);
            modelBuilder.Entity<Lot>().HasOne(l => l.Owner).WithMany(u => u.Lots);
            modelBuilder.Entity<Lot>().HasOne(l => l.Trade).WithOne(t=>t.TradeLot).HasForeignKey<Trade>(t=>t.Lot_Id);

            modelBuilder.Entity<Balance>().HasKey(b => b.Id);
            modelBuilder.Entity<Balance>().HasIndex("UserId","Currency").IsUnique();
            modelBuilder.Entity<Balance>().HasData
                (new Balance { Id = 1, Currency = "USD", Amount = 10 , UserId = user1.Id});
            modelBuilder.Entity<Balance>().HasData
                (new Balance { Id = 2, Currency = "USD", Amount = 20 , UserId = user2.Id});
        }
    }
}
