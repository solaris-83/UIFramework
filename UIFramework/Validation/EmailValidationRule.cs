using System.Text.RegularExpressions;
using UIFramework.Interfaces;


namespace UIFramework.Validation
{
    public class EmailValidationRule : ValidationRule
    {
        private readonly string _customErrorInfo;
        public EmailValidationRule()
        {
            
        }
        public EmailValidationRule(string errorInfo)
        {
            _customErrorInfo = errorInfo;
        }

        public override bool Validate(object value)
        {
            if (value == null)
                return false;

            ErrorInfo = null;
            bool ok = Regex.IsMatch(value.ToString(), @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            if (!ok)
            {
                ErrorInfo = string.IsNullOrEmpty(_customErrorInfo) ? "Invalid email format." : _customErrorInfo; // TODO MARCO: localize this message
               
                return false;
            }
            return true;
        }
    }
}
