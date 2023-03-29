using CurrencyTrading.Models;
using Microsoft.EntityFrameworkCore;

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

            modelBuilder.Entity<Trade>().HasKey(t => t.Id);
            modelBuilder.Entity<Trade>().HasOne(t => t.TradeLot).WithOne(l => l.Trade);
            modelBuilder.Entity<Trade>().HasOne(t => t.Buyer).WithMany(u=> u.Trades);

            modelBuilder.Entity<Lot>().HasKey(l => l.Id);
            modelBuilder.Entity<Lot>().HasOne(l => l.Owner).WithMany(u => u.Lots);
            modelBuilder.Entity<Lot>().HasOne(l => l.Trade).WithOne(t=>t.TradeLot).HasForeignKey<Trade>(t=>t.Lot_Id);

            modelBuilder.Entity<Balance>().HasKey(b => b.Id);
            modelBuilder.Entity<Balance>().HasIndex("UserId","Currency").IsUnique();
        }
    }
}
