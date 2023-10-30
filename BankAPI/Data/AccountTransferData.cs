using BankAPI.Data.Enums;

namespace BankAPI.Data
{
    public class AccountTransferData
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Money { get; set; }
        public Currency FromCurrency { get; set; }
        public Currency ToCurrency { get; set; }

        public override string ToString()
        {
            return $"[AT] From: {FromAccountId}(A); To: {ToAccountId}(A); Amount: {Money} {FromCurrency} -> {ToCurrency}";
        }
    }
}
