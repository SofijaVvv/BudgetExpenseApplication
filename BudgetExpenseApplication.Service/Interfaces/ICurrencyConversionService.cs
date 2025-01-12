using BudgetExpenseSystem.Model.Enum;

namespace BudgetExpenseApplication.Service.Interfaces;

public interface ICurrencyConversionService
{
	Task<decimal> GetExchangeRateAsync(CurrencyCode fromCurrency, CurrencyCode toCurrency);
}