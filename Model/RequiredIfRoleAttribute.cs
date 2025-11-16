// using System;
using System.ComponentModel.DataAnnotations;

namespace AuthTest.Model
{
    public class RequiredIfRoleAttribute : ValidationAttribute
    {
        private readonly string _role; // the role that triggers this requirement

        public RequiredIfRoleAttribute(string role)
        {
            _role = role;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            // Find the "Role" property on the model being validated
            var roleProp = context.ObjectType.GetProperty("Role");
            var role = roleProp?.GetValue(context.ObjectInstance)?.ToString();

            // If role matches and value is empty, validation fails
            if (role == _role && string.IsNullOrEmpty(value?.ToString()))
            {
                return new ValidationResult($"{context.DisplayName} is required for role {_role}");
            }

            return ValidationResult.Success;
        }
    }
}