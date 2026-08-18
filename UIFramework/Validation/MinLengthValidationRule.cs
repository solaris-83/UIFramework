using UIFramework.Interfaces;

namespace UIFramework.Validation
{
    public class MinLengthValidationRule : ValidationRule
    {
        private readonly string _customErrorInfo;
        public int MinLength { get; set; }
        public MinLengthValidationRule(int minLength, string errorInfo)
        {
            MinLength = minLength;
            _customErrorInfo = errorInfo;
        }
        public MinLengthValidationRule(int minLength) : this(minLength, null)
        {

        }
        public override bool Validate(object value)
        {
            if (value == null)
                return false;

            ErrorInfo = null;
            bool ok = value.ToString().Length >= MinLength;
            if (!ok)
            {
                ErrorInfo = string.IsNullOrEmpty(_customErrorInfo) ? $"Value must be at least {MinLength} characters long." : _customErrorInfo; // TODO MARCO: localize this message
                return false;
            }
            return true;
        }
    }
}
