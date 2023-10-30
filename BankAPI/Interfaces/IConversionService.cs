using BankAPI.Data.Enums;

namespace BankAPI.Interfaces
{
    public interface IConversionService
    {
        public decimal Convert(decimal money, Currency currencyFrom, Currency currencyTo);
        public Task<decimal> ConvertAsync(decimal money, Currency currencyFrom, Currency currencyTo);
    }
}
