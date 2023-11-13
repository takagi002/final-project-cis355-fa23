using System.ComponentModel.DataAnnotations;
using System.Reflection;

#nullable disable warnings
/// <summary>
/// Validates that the role entered is a field in the RoleNames class.
/// </summary>
public class RoleValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value != null)
        {
            string? role = value?.ToString();

            var fields = typeof(RoleNames).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            var constants = fields
                .Where(f => f.IsLiteral && !f.IsInitOnly)
                .Select(f => f.GetValue(null).ToString())
                .ToList();

            if (!constants.Contains(role))
            {
                return new ValidationResult("Role must be either 'Member' or 'Admin'.");
            }

            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult("Role must be either 'Member' or 'Admin'.");
        }
    }
}
