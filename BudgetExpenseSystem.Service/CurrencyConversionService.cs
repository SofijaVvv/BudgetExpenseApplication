using System.Text.Json;
using BudgetExpenseSystem.Model.Enum;
using BudgetExpenseSystem.Service.Dto;
using BudgetExpenseSystem.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BudgetExpenseSystem.Service;

public class CurrencyConversionService : ICurrencyConversionService
{
	private readonly string _apiKey;
	private readonly string _apiUrl;
	private readonly HttpClient _httpClient;
	private readonly ILogger<CurrencyConversionService> _logger;


	public CurrencyConversionService(HttpClient httpClient, IConfiguration configuration,
		ILogger<CurrencyConversionService> logger)
	{
		_httpClient = httpClient;
		_apiUrl = configuration["ExchangeRatesAPI:BaseUrl"] ??
		          throw new Exception("ExchangeRatesAPI:BaseUrl not found in configuration");
		_apiKey = configuration["ExchangeRatesAPI:ApiKey"] ??
		          throw new Exception("ExchangeRatesAPI:ApiKey");
		_logger = logger;
	}

	public async Task<decimal> GetExchangeRateAsync(CurrencyCode fromCurrency, CurrencyCode toCurrency)
	{
		var response = await _httpClient.GetAsync($"{_apiUrl}?access_key={_apiKey}");
		_logger.LogInformation($"Received HTTP response with status code: {response.StatusCode}");
		if (!response.IsSuccessStatusCode) throw new Exception("Failed to fetch exchange rates.");

		var jsonResponse = await response.Content.ReadAsStringAsync();
		_logger.LogInformation($"API Response Body: {jsonResponse}");

		var options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true // this might be the default anyway
		};

		var exchangeRates = JsonSerializer.Deserialize<ExchangeRatesResponse>(jsonResponse, options);
		_logger.LogInformation($"API exchange rates: {JsonSerializer.Serialize(exchangeRates)}");

		if (exchangeRates == null || !exchangeRates.Rates.ContainsKey(fromCurrency.ToString()) ||
		    !exchangeRates.Rates.ContainsKey(toCurrency.ToString())) throw new Exception("Invalid currency codes.");


		var rateFrom = exchangeRates.Rates[fromCurrency.ToString()];
		var rateTo = exchangeRates.Rates[toCurrency.ToString()];

		return rateTo / rateFrom;
	}
}