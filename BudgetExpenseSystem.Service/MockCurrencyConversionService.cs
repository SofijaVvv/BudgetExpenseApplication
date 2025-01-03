using BudgetExpenseSystem.Service.Interfaces;
using Microsoft.Extensions.Logging;

namespace BudgetExpenseSystem.Service;

public class MockCurrencyConversionService : ICurrencyConversionService
{
	private readonly ILogger<MockCurrencyConversionService> _logger;

	public MockCurrencyConversionService(ILogger<MockCurrencyConversionService> logger)
	{
		_logger = logger;
	}

	public Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency)
	{
		var mockRates = new Dictionary<string, decimal>
		{
			{ "USD", 1.0m },
			{ "EUR", 0.93m },
			{ "GBP", 0.81m }
		};

		if (!mockRates.ContainsKey(fromCurrency) || !mockRates.ContainsKey(toCurrency))
			throw new Exception("Invalid currency codes.");

		var rateFrom = mockRates[fromCurrency];
		var rateTo = mockRates[toCurrency];
		var exchangeRate = rateTo / rateFrom;

		_logger.LogInformation($"Mocked exchange rate: {exchangeRate} from {fromCurrency} to {toCurrency}");

		return Task.FromResult(exchangeRate);
	}
}
