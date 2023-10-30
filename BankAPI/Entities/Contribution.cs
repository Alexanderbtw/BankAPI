using BankAPI.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAPI.Entities
{
    public class Contribution
    {
        public int Id { get; set; }

        public DateOnly EndDate { get; set; }

        [NotMapped]
        public TimeSpan Duration { get; set; }

        public float Percents { get; set; }

        public int? AccountId { get; set; }

        public Account? ParentAccount { get; set; }

        [Column("money")]
        public decimal Money { get; set; }

        public Currency Currency { get; set; }

        public int OwnerId { get; set; }


        internal void CalculeteProfit()
        {
            int month = this.Duration.Days / 30;
            for (int i = 0; i < month; i++)
            {
                this.Money += Math.Round((this.Money * ((decimal)this.Percents / 12)) / 100, 2);
            }
        }

        public override string ToString()
        {
            return $"[Co] ID: {Id}; Money: {Money}; Duration: {Duration}";
        }
    }
}
