using System.ComponentModel.DataAnnotations;

namespace InvoiceAPIChallenge.Controllers
{
    /// <summary>
    /// The validation attributes in this class are not actually necessary because 
    /// it represents data that the API sends back to the client. 
    /// However, just in case to ensure proper formatting or set constraints on the data that the API generates.
    /// </summary>
    public class InvoiceOutput
    {
        [Range(0, double.MaxValue, ErrorMessage = "Invalid Pre-Tax Amount")]
        public decimal PreTaxAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Invalid Tax Amount")]
        public decimal TaxAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Invalid Grand Total")]
        public decimal GrandTotal { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Invalid Exchange Rate")]
        public decimal ExchangeRate { get; set; }
    }
}
