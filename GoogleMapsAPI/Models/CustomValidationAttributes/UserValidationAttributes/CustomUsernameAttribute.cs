using Microsoft.IdentityModel.Tokens;

using System.ComponentModel.DataAnnotations;

namespace GoogleMapsAPI.Models.CustomValidationAttributes.UserValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class CustomUsernameValidation : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string username && value is not null)
            {
                if (username.Length < 8 && username.Length > 20 && username.IsNullOrEmpty())
                    return new ValidationResult("Username must be 8 characters long and at most 20.");
                if (username.Any(char.IsWhiteSpace))
                    return new ValidationResult("Spaces are not allowed.");
                if (username.FirstOrDefault() is char firstChar && !char.IsLetter(firstChar))
                    return new ValidationResult("Username must start with a Letter");
                if (!username.All(char.IsLetterOrDigit))
                    return new ValidationResult("Username can only contain Character and Number");
                return ValidationResult.Success;
            }
            return new ValidationResult("Null value or malformed value was passed");
        }
    }

}
