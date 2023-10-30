using BankAPI.Data.Enums;
using Npgsql.Internal.TypeHandlers.NumericHandlers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text;

namespace BankAPI.Entities
{
    public class Account
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [Column("money")]
        public decimal Money { get; set; }

        public Currency Currency { get; set; }

        public int OwnerId { get; set; }

        public override string ToString()
        {
            return $"[A] ID: {Id}";
        }
    }
}
