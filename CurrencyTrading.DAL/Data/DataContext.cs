using CurrencyTrading.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Web.Helpers;

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
            modelBuilder.Entity<User>().HasMany(u => u.Lots).WithOne(l=>l.Owner);
            modelBuilder.Entity<User>().HasMany(u => u.Trades).WithOne(l=>l.Buyer);
            User user1 = new User
            {
                Id = 1,  
                Login = "test1",
                Password = Crypto.HashPassword("test1"),
            };
            User user2 = new User
            {
                Id = 2,
                Login = "test2",
                Password = Crypto.HashPassword("test2"),
            };
            modelBuilder.Entity<User>().HasData(user1);
            modelBuilder.Entity<User>().HasData(user2);

            modelBuilder.Entity<Trade>().HasKey(t => t.Id);
            modelBuilder.Entity<Trade>().HasOne(t => t.TradeLot).WithOne(l => l.Trade);
            modelBuilder.Entity<Trade>().HasOne(t => t.Buyer).WithMany(u=> u.Trades);

            modelBuilder.Entity<Lot>().HasKey(l => l.Id);
            modelBuilder.Entity<Lot>().HasOne(l => l.Owner).WithMany(u => u.Lots);
            modelBuilder.Entity<Lot>().HasOne(l => l.Trade).WithOne(t=>t.TradeLot).HasForeignKey<Trade>(t=>t.LotId);

            modelBuilder.Entity<Balance>().HasKey(b => b.Id);
            modelBuilder.Entity<Balance>().HasIndex("UserId","Currency").IsUnique();
            modelBuilder.Entity<Balance>().HasOne(b => b.User).WithMany(u => u.Balance);
            Balance balance1 = new Balance { Id = 1, Currency = "RUB", Amount = 10000, UserId = 1 };
            Balance balance2 = new Balance { Id = 2, Currency = "RUB", Amount = 10000, UserId = 2 };
            modelBuilder.Entity<Balance>().HasData(balance1);
            modelBuilder.Entity<Balance>().HasData(balance2);
        }
    }
}
