
namespace InvoiceAPIChallenge.Models
{
    public class ExchangeAPIResponse
    {
        public bool Success { get; set; }
        public Dictionary<string, decimal> Rates { get; set; } = [];
    }
}
