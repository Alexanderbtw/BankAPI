using BankAPI.Data.Enums;
using BankAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace BankAPI.Data.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(a => a.Id).ValueGeneratedOnAdd();
            builder.Property(b => b.Currency).HasConversion(c => c.ToString(), c => Enum.Parse<Currency>(c));
            builder.ToTable(t => t.HasCheckConstraint(nameof(Account.Money), "Money >= 0"));
            builder.HasIndex(a => new { a.Title, a.OwnerId }).IsUnique();
        }
    }
}
