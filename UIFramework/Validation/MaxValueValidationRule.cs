
using UIFramework.Interfaces;

namespace UIFramework.Validation
{
    public class MaxValueValidationRule : ValidationRule
    {
        private readonly string _customErrorInfo;
        public double? MaxValue { get; set; }
        public MaxValueValidationRule(double maxValue, string errorInfo)
        {
            MaxValue = maxValue;
            _customErrorInfo = errorInfo;
        }
        public MaxValueValidationRule(double maxValue) : this(maxValue, null)
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
            if (d > MaxValue)
            {
                ErrorInfo = string.IsNullOrEmpty(_customErrorInfo) ? $"Value cannot be greater than {MaxValue}." : _customErrorInfo; // TODO MARCO: localize this message
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
