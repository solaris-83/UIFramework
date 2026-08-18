using System;
using UIFramework.Interfaces;

namespace UIFramework.Validation
{
    public class RangeValidationRule : ValidationRule
    {
        private readonly string _customErrorInfo;
        public double Min { get; }
        public double Max { get; }

        public RangeValidationRule(double min, double max, string errorInfo)
        {
            Min = min;
            Max = max;
            _customErrorInfo = errorInfo;
        }

        public RangeValidationRule(double min, double max) : this(min, max, null)
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
                ErrorInfo = $"Value {value} cannot be converted to number."; // TODO MARCO: localize this message
                return false;
            }
            ok = d >= Min && d <= Max;
            if (!ok)
            {
                ErrorInfo = string.IsNullOrEmpty(_customErrorInfo) ? $"Value must be between {Min} and {Max}." : _customErrorInfo; // TODO MARCO: localize this message
                return false;
            }
            return true;
        }
    }
}
