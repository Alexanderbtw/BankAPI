using BankAPI.Loggers;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Database
{
    public class AuditContext : DbContext
    {
        public DbSet<Audit> AuditRecords { get; set; }

        public AuditContext(DbContextOptions<AuditContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
