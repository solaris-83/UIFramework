using UIFramework.Interfaces;

namespace UIFramework.Validation
{
    public class MinValueValidationRule : ValidationRule
    {
        private readonly string _customErrorInfo;
        public double? MinValue { get; set; }
        public MinValueValidationRule(double minValue, string errorInfo)
        {
            MinValue = minValue;
            _customErrorInfo = errorInfo;
        }

        public MinValueValidationRule(double minValue) : this (minValue, null)
        {
           
        }

        public override bool Validate(object value)
        {
            if (value == null)
                return false;

            ErrorInfo = null;
            
            bool ok = double.TryParse(value.ToString(), out double d);
            if (!ok)
            {
                ErrorInfo = "Value must be numeric."; // TODO MARCO: localize this message
                return false;
            }
            if (d < MinValue)
            {
                ErrorInfo = string.IsNullOrEmpty(_customErrorInfo) ? $"Value cannot be lower than {MinValue}." : _customErrorInfo; // TODO MARCO: localize this message
                
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
