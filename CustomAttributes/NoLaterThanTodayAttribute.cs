
using System.ComponentModel.DataAnnotations;

namespace InvoiceAPIChallenge.CustomAttributes
{/// <summary>
 /// Enforces a date constraint - The date must be set in the present or the past, not the future.
 /// </summary>
    public class NoLaterThanTodayAttribute : ValidationAttribute
    {
        public NoLaterThanTodayAttribute()
        {
            ErrorMessage ??= "The date cannot be in the future.";
        }

        /// <summary>
        /// Ensure the date provided is no later than the current date
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateValue)
            {
                if (dateValue > DateTime.UtcNow)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            else
            {
                return new ValidationResult("Invalid date format.");
            }

            return ValidationResult.Success;
        }
    }
}
