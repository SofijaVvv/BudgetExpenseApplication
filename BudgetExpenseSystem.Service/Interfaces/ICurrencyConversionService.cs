namespace BudgetExpenseSystem.Service.Interfaces;

public interface ICurrencyConversionService
{
	Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency);
}
