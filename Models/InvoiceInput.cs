using InvoiceAPIChallenge.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace InvoiceAPIChallenge.Models
{
    /// <summary>
    /// Represents the input data for an invoice calculation request.
    /// This class includes details such as the invoice date, the pre-tax amount in Euros (EUR),
    /// and the currency in which the payment is to be made.
    /// The invoice date should not be in the future, and the pre-tax amount should be a positive value.
    /// The payment currency is used to determine the appropriate exchange rate and tax rate calculations.
    /// Supported currencies for conversion include CAD (Canadian Dollar) and USD (US Dollar).
    /// </summary>
    public class InvoiceInput
    {
        [Required]
        [DataType(DataType.Date)]
        [YearLimit(10), NoLaterThanToday]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Pre-Tax Amount must be greater than 0")]
        public decimal PreTaxAmountEUR { get; set; }

        [Required]
        [RegularExpression("^(EUR|USD|CAD)$", ErrorMessage = "Payment Currency must be either 'EUR', 'CAD', or 'USD'")]
        public string PaymentCurrency { get; set; } = string.Empty;
    }
}
