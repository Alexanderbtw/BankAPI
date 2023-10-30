using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAPI.Entities
{
    public class Avatar
    {
        public int Id { get; set; }

        public byte[] Image { get; set; }
    }
}
