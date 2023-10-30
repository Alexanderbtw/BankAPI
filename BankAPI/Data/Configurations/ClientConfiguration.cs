using BankAPI.Data.Enums;
using BankAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace BankAPI.Data.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.HasIndex(c => c.Username).IsUnique();
            builder.Property(c => c.Status).HasDefaultValue(Status.Individual);
            builder.HasMany(c => c.Accounts).WithOne().HasForeignKey(a => a.OwnerId).IsRequired().OnDelete(DeleteBehavior.SetNull);
            builder.Property(b => b.Status).HasConversion(c => c.ToString(), c => Enum.Parse<Status>(c));
            builder.HasMany(c => c.Contributions).WithOne().HasForeignKey(c => c.OwnerId).IsRequired().OnDelete(DeleteBehavior.SetNull);
        }
    }
}
