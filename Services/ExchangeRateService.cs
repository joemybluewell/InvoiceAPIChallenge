using InvoiceAPIChallenge.Controllers;
using InvoiceAPIChallenge.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;


namespace InvoiceAPIChallenge.Services
{
    /// <summary>
    /// Create the service interface to allow Unit Testing. 
    /// This is needed because Moq only handles parameterless constructors.
    /// </summary>
    /// 
    public interface IExchangeRateService
    {
        /// <summary>
        /// Gets the exchange rate for a specified date and currency pair.
        /// </summary>
        /// <param name="baseCurrency">Base currency code (e.g., EUR).</param>
        /// <param name="targetCurrency">Target currency code (e.g., USD).</param>
        /// <param name="date">Date for which to retrieve the exchange rate.</param>
        /// <returns>The exchange rate as a decimal.</returns>
        /// <exception cref="InvalidOperationException">Thrown when unable to retrieve the exchange rate.</exception>
        Task<decimal> GetExchangeRate(string baseCurrency, string targetCurrency, DateTime date);
    }


    /// <summary>
    /// Service class for fetching exchange rates from an external API
    /// </summary>
    /// <param name="httpClient">Injected from the main class</param>
    /// <param name="settings">Injected from the main class</param>
    public class ExchangeRateService(HttpClient httpClient, IOptions<ExternalSettings> settings) : IExchangeRateService
    {
        private readonly ExternalSettings exchangeApiInfo = settings?.Value ??
            throw new ArgumentNullException(nameof(settings)); 
        


        public async Task<decimal> GetExchangeRate(string baseCurrency, string targetCurrency, DateTime date)
        {
            // Return 1 if no conversion is needed
            if (baseCurrency == targetCurrency)
            {
                return 1m;
            }
            ArgumentNullException.ThrowIfNull(httpClient);

            // Construct the request URI using settings and parameters
            string requestUri = $"{exchangeApiInfo.ExchangeApiUrl}{date:yyyy-MM-dd}?access_key={exchangeApiInfo.ExchangeApiKey}&base={baseCurrency}&symbols={targetCurrency}";

            // Make an HTTP GET request to the external API
            HttpResponseMessage response = await httpClient.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var currencyApiResponse = JsonConvert.DeserializeObject<ExchangeAPIResponse>(json);

                // Check if response is successful and contains the requested rate
                if (currencyApiResponse != null && currencyApiResponse.Success && currencyApiResponse.Rates.TryGetValue(targetCurrency, out decimal rate))
                {
                    return rate;
                }
            }

            throw new InvalidOperationException($"Failed to retrieve exchange rate from {exchangeApiInfo.ExchangeApiUrl}");
        }
    }
}
