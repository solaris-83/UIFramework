
using UIFramework.Interfaces;

namespace UIFramework.Validation
{
    public class ExactLengthValidationRule : ValidationRule
    {
        private readonly string _customErrorInfo;

        public int Length { get; }
        public ExactLengthValidationRule(int length, string errorInfo)
        {
            Length = length;
            _customErrorInfo = errorInfo;
        }
        public ExactLengthValidationRule(int length) : this(length, null)
        {

        }

        public override bool Validate(object value)
        {
            if (value == null)
                return false;

            ErrorInfo = null;
            bool ok = value.ToString().Length <= Length;
            if (!ok)
            {
                ErrorInfo = string.IsNullOrEmpty(_customErrorInfo) ? $"Value must be exactly {Length} characters long." : _customErrorInfo; // TODO MARCO: localize this message
                return false;
            }
            return true;
        }
    }
}
