using InvoiceAPIChallenge.Models;
using InvoiceAPIChallenge.Services;
using Microsoft.AspNetCore.Mvc;


namespace InvoiceAPIChallenge.Controllers
{
    /// <summary>
    /// Controller responsible for handling invoice calculations.
    /// This class uses the primary constructor feature of .NET 6, which allows for the direct declaration of dependencies as constructor parameters.
    /// </summary>
    /// <param name="logger">Logger for logging information and errors.</param>
    /// <param name="exchangeRateService">Service to handle exchange rate calculations.</param>
    [ApiController]
    [Route("api/v1/invoices")]
    public class InvoiceController(ILogger<InvoiceController> logger, IExchangeRateService exchangeRateService) : ControllerBase
    {


        /// <summary>
        /// Calculates the tax and total amount for a given invoice based on the exchange rate on a specific date.
        /// </summary>
        /// <param name="input">Invoice input details including date, pre-tax amount in EUR, and payment currency.</param>
        /// <returns>Returns an IActionResult containing the calculated invoice details or an error message.</returns>
        [HttpPost("calculate")]
        public async Task<IActionResult> CalculateInvoice([FromBody] InvoiceInput input)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Fetch the exchange rate
                var exchangeRate = await exchangeRateService.GetExchangeRate("EUR", input.PaymentCurrency, input.InvoiceDate);

                // Calculate the amounts
                var preTaxAmount = input.PreTaxAmountEUR * exchangeRate;
                var taxRate = GetTaxRate(input.PaymentCurrency);
                var taxAmount = preTaxAmount * taxRate;
                var grandTotal = preTaxAmount + taxAmount;

                // Create the response object
                var output = new InvoiceOutput
                {
                    PreTaxAmount = Math.Round(preTaxAmount, 2),
                    TaxAmount = Math.Round(taxAmount, 2),
                    GrandTotal = Math.Round(grandTotal, 2),
                    ExchangeRate = exchangeRate
                }; 
                return Ok(output);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error calculating invoice"); 
                return BadRequest(ex.Message);
            }
        }



        /// <summary>
        /// Retrieves the tax rate based on the provided currency.
        /// </summary>
        /// <param name="currency">The currency for which to determine the tax rate.</param>
        /// <returns>The tax rate as a decimal.</returns>
        private static decimal GetTaxRate(string currency)
        {
            return currency switch
            {
                "CAD" => 0.11m, // Tax rate for Canadian Dollar
                "USD" => 0.10m, // Tax rate for US Dollar
                _ => 0.09m,     // Default tax rate in Euro
            };
        }
    }
}
