using BudgetExpenseSystem.Model.Enum;

namespace BudgetExpenseSystem.Service.Interfaces;

public interface ICurrencyConversionService
{
	Task<decimal> GetExchangeRateAsync(CurrencyCode fromCurrency, CurrencyCode toCurrency);
}