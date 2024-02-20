using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Gamesoft.ViewModels
{
    public class LoginViewModel
    {
        [RequiredValue]
        [EmailAddress]
        public required string Email { get; set; }

        [RequiredValue]
        [PasswordPropertyText]        
        public required string Password { get; set; }
    }

    public class RequiredValueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                if (!string.IsNullOrEmpty((string)value))
                    return ValidationResult.Success;
                else
                    return new ValidationResult("La valeur est requise");
            }
            catch (Exception)
            {
                return new ValidationResult("Error.");
            }
        }
    }
}
