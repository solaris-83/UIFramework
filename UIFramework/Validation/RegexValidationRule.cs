using UIFramework.Interfaces;

namespace UIFramework.Validation
{
    public class RegexValidationRule : ValidationRule
    {
        private readonly string _customErrorInfo;
        public RegexValidationRule(string pattern) : this(pattern, null)
        {
        }

        public RegexValidationRule(string pattern, string errorInfo)
        {
            Pattern = pattern;
            _customErrorInfo = errorInfo;
        }

        public string Pattern { get; }

        public override bool Validate(object value)
        { 
            if (value == null)
                return false;

            ErrorInfo = null;
            bool ok = System.Text.RegularExpressions.Regex.IsMatch(value.ToString(), Pattern);
            if (!ok)
            {
                ErrorInfo = string.IsNullOrEmpty(_customErrorInfo) ? $"Value does not match the required pattern." : _customErrorInfo; // TODO MARCO: localize this message
                return false;
            }
            return true;
        }
    }
}
