
namespace UIFramework.Interfaces
{
    public interface IValidationRule
    {
        string Type { get; }
        bool Validate(object value);
        string ErrorInfo { get; set; }
    }

    public abstract class ValidationRule : IValidationRule
    {
        public string Type => GetType().Name;
        public string ErrorInfo { get; set; }
        public abstract bool Validate(object value);
    }
}
