using BudgetExpenseApplication.Service.Interfaces;
using BudgetExpenseSystem.Model.Enum;
using Microsoft.Extensions.Logging;

namespace BudgetExpenseApplication.Service.Mock;

public class MockCurrencyConversionService : ICurrencyConversionService
{
	private readonly ILogger<MockCurrencyConversionService> _logger;

	public MockCurrencyConversionService(ILogger<MockCurrencyConversionService> logger)
	{
		_logger = logger;
	}

	public Task<decimal> GetExchangeRateAsync(CurrencyCode fromCurrency, CurrencyCode toCurrency)
	{
		var mockRates = new Dictionary<string, decimal>
		{
			{ "USD", 1.0m },
			{ "EUR", 0.93m },
			{ "GBP", 0.81m }
		};

		if (!mockRates.ContainsKey(fromCurrency.ToString()) || !mockRates.ContainsKey(toCurrency.ToString()))
			throw new Exception("Invalid currency codes.");

		var rateFrom = mockRates[fromCurrency.ToString()];
		var rateTo = mockRates[toCurrency.ToString()];
		var exchangeRate = rateTo / rateFrom;

		_logger.LogInformation($"Mocked exchange rate: {exchangeRate} from {fromCurrency} to {toCurrency}");

		return Task.FromResult(exchangeRate);
	}
}