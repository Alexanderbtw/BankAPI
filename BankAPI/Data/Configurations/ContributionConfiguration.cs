using BankAPI.Data.Enums;
using BankAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace BankAPI.Data.Configurations
{
    public class ContributionConfiguration : IEntityTypeConfiguration<Contribution>
    {
        public void Configure(EntityTypeBuilder<Contribution> builder)
        {
            builder.ToTable(t => t.HasCheckConstraint(nameof(Contribution.Money), "Money >= 0"));
            builder.Property(c => c.Currency).HasConversion(c => c.ToString(), s => Enum.Parse<Currency>(s!));
            builder.HasOne(a => a.ParentAccount).WithMany().HasForeignKey(c => c.AccountId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
