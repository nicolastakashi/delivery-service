using DeliveryService.Infra.Security;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Infra.Attributes
{
    public sealed class PasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value.ToString())) return null;

                validationContext
                    .ObjectType
                    .GetProperty(validationContext.MemberName)
                    .SetValue(validationContext.ObjectInstance, SecurityString.Hash(value.ToString()), null);

                return null;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

    }
}
