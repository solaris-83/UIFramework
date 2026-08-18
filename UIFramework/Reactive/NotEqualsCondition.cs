using log4net;
using System;
using UIFramework.Helpers;
using UIFramework.Interfaces.Reactive;
using UIFramework.UIElements.Base;

namespace UIFramework.Reactive
{
    public class NotEqualsCondition : ICondition
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(NotEqualsCondition));
        private readonly UIElement _target;
        private readonly string _propertyName;
        private readonly object _expectedValue;

        public NotEqualsCondition(UIElement targetElement, string propertyName, object expectedValue)
        {
            _target = targetElement;
            _propertyName = propertyName;
            _expectedValue = expectedValue;
        }

        public bool Evaluate()
        {
            _logger.Debug($"Evaluating NotEqualsCondition for property '{_propertyName}' on element '{_target.ToString()}' with expected value '{_expectedValue}'");
            var currentValue = PropertyPathResolver.GetPropertyValue(_target, _propertyName) ?? throw new InvalidOperationException($"Property '{_propertyName}' not found");
            var convertedValue = Convert.ChangeType(currentValue, _expectedValue.GetType());
            var result = !Equals(convertedValue, _expectedValue);
            _logger.Debug($"NotEqualsCondition result for property '{_propertyName}' on element '{_target.ToString()}': {result}");
            return result;
        }

        public UIElement GetTargetElement()
        {
            return _target;
        }
    }
}
