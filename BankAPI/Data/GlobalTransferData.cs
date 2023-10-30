using BankAPI.Data.Enums;

namespace BankAPI.Data
{
    public class GlobalTransferData
    {
        public int AccountId { get; set; }
        public Currency AccountCurrency { get; set; }
        public decimal Money { get; set; }
        public float Commission { get; set; }
        public int RecipientId { get; set; }

        public override string ToString()
        {
            return $"[GT] From: {AccountId}(A); To: {RecipientId}(C); Amount: {Money}; Commission: {Commission}";
        }
    }
}
