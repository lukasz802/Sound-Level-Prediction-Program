using System.Globalization;
using System.Windows.Controls;

namespace SoundLevelCalculator.ValidationRules
{
    public class TextBoxValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string textboxValue = value.ToString();
            if (string.IsNullOrEmpty(textboxValue))
            {
                return new ValidationResult(false, "Pole nie może być puste");
            }

            return new ValidationResult(true, null);
        }
    }
}
