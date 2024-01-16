
namespace InvoiceAPIChallenge.Controllers
{
    /// <summary>
    /// Holds the information for the external API to query for currency exchange rates
    /// </summary>
    public class ExternalSettings
    {
        public string ExchangeApiUrl { get; set; } = string.Empty;
        public string ExchangeApiKey { get; set; } = string.Empty;
    }
}
