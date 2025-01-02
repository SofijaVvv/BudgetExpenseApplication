using System.Text.Json;
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

	public async Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency)
	{
		var response = await _httpClient.GetAsync($"{_apiUrl}?access_key={_apiKey}");
		_logger.LogInformation($"Received HTTP response with status code: {response.StatusCode}");
		if (!response.IsSuccessStatusCode) throw new Exception("Failed to fetch exchange rates.");

		var jsonResponse = await response.Content.ReadAsStringAsync();
		_logger.LogInformation($"API Response Body: {jsonResponse}");

		var options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
		};

		var exchangeRates = JsonSerializer.Deserialize<ExchangeRatesResponse>(jsonResponse, options);
		_logger.LogInformation($"API exchange rates: {JsonSerializer.Serialize(exchangeRates)}");


		if (exchangeRates == null || !exchangeRates.Rates.ContainsKey(fromCurrency) ||
		    !exchangeRates.Rates.ContainsKey(toCurrency)) throw new Exception("Invalid currency codes.");


		var rateFrom = exchangeRates.Rates[fromCurrency];
		var rateTo = exchangeRates.Rates[toCurrency];

		_logger.LogInformation($"Rate From: {rateFrom}, Rate To: {rateTo}");

		var result = rateTo / rateFrom;

		_logger.LogInformation($"rateFrom devided with rateTo result {result}");
		return result;
	}


	private class ExchangeRatesResponse
	{
		public bool Success { get; set; }
		public int Timestamp { get; set; }
		public string Base { get; set; }
		public string Date { get; set; }
		public Dictionary<string, decimal> Rates { get; set; }
	}

}
