using BankAPI.Data.Configurations;
using BankAPI.Data.Enums;
using BankAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Database
{
    public class BankContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<Avatar> Avatars { get; set; }

        public BankContext(DbContextOptions<BankContext> options) : base(options) 
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Avatar>().Property(a => a.Id).ValueGeneratedOnAdd();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BankContext).Assembly);
        }

        public override ValueTask DisposeAsync() => base.DisposeAsync();
    }
}
