using UIFramework.Interfaces;

namespace UIFramework.Validation
{
    public class MaxLengthValidationRule : ValidationRule
    {
        private readonly string _customErrorInfo;
        public int MaxLength { get; set; }
        public MaxLengthValidationRule(int maxLength, string errorInfo)
        {
            MaxLength = maxLength;
            _customErrorInfo = errorInfo;
        }
        public MaxLengthValidationRule(int maxLength) : this(maxLength, null)
        {

        }

        public override bool Validate(object value)
        {
            if (value == null)
                return false;

            ErrorInfo = null;
            bool ok = value.ToString().Length <= MaxLength;
            if (!ok)
            {
                ErrorInfo = string.IsNullOrEmpty(_customErrorInfo) ? $"Value must be at most {MaxLength} characters long." : _customErrorInfo; // TODO MARCO: localize this message
                return false;
            }
            return true;
        }
    }
}
