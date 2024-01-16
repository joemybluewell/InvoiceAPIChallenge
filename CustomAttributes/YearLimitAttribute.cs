
using System.ComponentModel.DataAnnotations;

namespace InvoiceAPIChallenge.CustomAttributes
{
    /// <summary>
    /// Enforces a given date to be earlier than the given number of years.
    /// </summary>
    /// <param name="years">The number of years provided by the dev user.</param>
    public class YearLimitAttribute(int years) : ValidationAttribute
    {
        /// <summary>
        /// Processes the validation.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns>True if the condition is fulfilled.</returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateValue)
            {
                if (dateValue < DateTime.UtcNow.AddYears(-years))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            else
            {
                return new ValidationResult("Invalid date format.");
            }

            return ValidationResult.Success;
        }


        /// <summary>
        /// Provides the error message for the validation
        /// </summary>
        /// <returns>A string message</returns>
        private string GetErrorMessage()
        {
            return $"Date cannot be earlier than {years} years from today.";
        }
    }

}
