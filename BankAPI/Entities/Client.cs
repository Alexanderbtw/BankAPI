using BankAPI.Data.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

namespace BankAPI.Entities
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        public Status Status { get; set; }

        public Avatar? Avatar { get; set; }

        public ICollection<Account> Accounts { get; set; } = new Collection<Account>();

        public ICollection<Contribution> Contributions { get; set; } = new Collection<Contribution>();

        public override string ToString()
        {
            return $"[Cl] ID: {Id}";
        }
    }
}
